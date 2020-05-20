using Microsoft.AspNetCore.Mvc;
using userService.Model;
using userService.Repository;
using System;
using System.Transactions;

using Microsoft.Extensions.Configuration;
using userService.Service;
using Microsoft.AspNetCore.Authorization;

namespace userService.Controllers
{
  [Route("perime-user-ms/[controller]")]
  [ApiController]
  public class SessionmController : ControllerBase
  {

    private readonly IUserRepository _userRepository;
    private readonly ISessionmRepository _sessionRepository;
    private IConfiguration _config;
    public SessionmController (IUserRepository userRepository, ISessionmRepository sessionmRepository, IConfiguration config)
    {
      _userRepository = userRepository;
      _sessionRepository = sessionmRepository;
      _config = config;
    }

    [HttpGet]
    public IActionResult Get()
    {
      try
      {
      var sessions = _sessionRepository.GetSessions();
      return new OkObjectResult(sessions);
      }
      catch(Exception)
      {
        return new StatusCodeResult(500);
      }
    }


    [HttpPost]
    public IActionResult Post([FromBody] Login login)
    {
      try
      {
          var userGot =_userRepository.GetUserByEmail(login.email);
          if (userGot != null)
          {
          
            if (_userRepository.CheckMatch(userGot.passhash_user, login.password) && _userRepository.CheckLDAP(login.email, login.password))
            {
              var jwt = new JwtService(_config);
              var token = jwt.GenerateSecurityToken(userGot);

              Sessionm newSession = new Sessionm();
              newSession.id_session = userGot.id_user;
              newSession.token_session = token;
              using (var scope = new TransactionScope())
              {
                var alreadySession = _sessionRepository.GetSessionById(userGot.id_user);
                if (alreadySession != null)
                {
                  _sessionRepository.DeleteSession(userGot.id_user);
                }
                _sessionRepository.InsertSession(newSession);
                scope.Complete();
              }
              return new OkObjectResult(newSession);
            }
            else
            {
              return new NotFoundResult();
            }
          }
          else
          {
            return new NotFoundResult();
          }   
      }
      catch(Exception)
      {
          return new StatusCodeResult(500);
      } 
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
      try
      {
        var sessionDeleted = _sessionRepository.GetSessionById(id);
        if (sessionDeleted != null)
        {
          _sessionRepository.DeleteSession(id);
          return new OkResult();
        }
        else
        {
          return new BadRequestResult();
        }
      }
      catch(Exception)
      {
        return new StatusCodeResult(500);
      }
    }

    [Authorize]
    [HttpGet("{id}")]
    public IActionResult Get([FromHeader (Name="Authorization") ] string token, int id)
    {
      try
      {
        string checkToken = token.Substring(7);
        var session = _sessionRepository.GetSessionById(id);
        if (session != null && session.token_session.Equals(checkToken)){
          return new OkResult();
        }
        else
        {
          return new BadRequestResult();
        }
      }
      catch(Exception)
      {
        return new StatusCodeResult(500);
      }
    }


  }
}