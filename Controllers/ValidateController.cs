using Microsoft.AspNetCore.Mvc;
using userService.Repository;
using System;

using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace userService.Controllers
{
  [Route("perime-user-ms/[controller]")]
  [ApiController]
  public class ValidateController : ControllerBase
  {

    private readonly IUserRepository _userRepository;
    private readonly ISessionmRepository _sessionmRepository;
    private readonly ISessionwRepository _sessionwRepository;
    private IConfiguration _config;
    public ValidateController (IUserRepository userRepository, ISessionmRepository sessionmRepository, ISessionwRepository sessionwRepository, IConfiguration config)
    {
        _userRepository = userRepository;
        _sessionmRepository = sessionmRepository;
        _sessionwRepository = sessionwRepository;
        _config = config;
    }

    [Authorize]
    [HttpGet("{id}")]
    public IActionResult Get([FromHeader (Name="Authorization") ] string token, int id)
    {
      try
      {
        string checkToken = token.Substring(7);
        var sessionm = _sessionmRepository.GetSessionById(id);
        var sessionw = _sessionwRepository.GetSessionById(id);
        if ((sessionm != null && sessionm.token_session.Equals(checkToken)) || (sessionw != null && sessionw.token_session.Equals(checkToken))){
          return new OkResult();
        }
        else
        {
          return new UnauthorizedResult();
        }
      }
      catch(Exception)
      {
        return new StatusCodeResult(500);
      }
    }

  }
}