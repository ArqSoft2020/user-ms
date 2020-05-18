using Microsoft.AspNetCore.Mvc;
using userService.Model;
using userService.Repository;
using userService.Controllers;
using System;
using System.Collections.Generic;
using System.Transactions;

using Microsoft.Extensions.Configuration;
using userService.Service;
using Microsoft.AspNetCore.Authorization;

namespace userService.Controllers
{
  [Route("perime-user-ms/[controller]")]
  [ApiController]
  public class SessionController : ControllerBase
  {

    private readonly IUserRepository _userRepository;
    private readonly ISessionRepository _sessionRepository;
    private IConfiguration _config;
    public SessionController (IUserRepository userRepository, ISessionRepository sessionRepository, IConfiguration config)
    {
      _userRepository = userRepository;
      _sessionRepository = sessionRepository;
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

              Session newSession = new Session();
              newSession.id_session = userGot.id_user;
              newSession.token_session = token;
              using (var scope = new TransactionScope())
              {
                _sessionRepository.InsertSession(newSession);
                scope.Complete();
              }
              return new OkObjectResult(newSession);
            }
            else
            {
              return Content("Invalid Password");
            }
          }
          else
          {
            return Content("Email not found!");
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
        _sessionRepository.DeleteSession(id);
        return new OkObjectResult(sessionDeleted);
      }
      catch(Exception)
      {
        return new StatusCodeResult(500);
      }
    }

    [Authorize]
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
      try
      {
        var session = _sessionRepository.GetSessionById(id);
        if (session != null){
          return new OkResult();
        }
        else
        {
          return new StatusCodeResult(401);
        }
      }
      catch(Exception)
      {
        return new StatusCodeResult(500);
      }
    }


  }
}