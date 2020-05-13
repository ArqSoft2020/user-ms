using Microsoft.AspNetCore.Mvc;
using userService.Model;
using userService.Repository;
using System;
using System.Collections.Generic;
using System.Transactions;
using Microsoft.AspNetCore.Authorization;

namespace userService.Controllers
{
  [Route("perime-user-ms/[controller]")]
  [ApiController]
  public class UserController : ControllerBase
  {

    private readonly IUserRepository _userRepository;

    public UserController (IUserRepository userRepository)
    {
      _userRepository = userRepository;
    }

    [Authorize]
    [HttpGet]
    public IActionResult Get()
    {
      try
      {
      var users = _userRepository.GetUsers();
      return new OkObjectResult(users);
      }
      catch(Exception)
      {
        return new StatusCodeResult(500);
      }
    }

/*
    [HttpGet("{id}", Name = "Get")]
    public IActionResult Get(int id)
    {
      try
      {
        var user = _userRepository.GetUserById(id);
        return new OkObjectResult(user);
      }
      catch(Exception)
      {
        return new StatusCodeResult(500);
      }
      
    }
*/
    [Authorize]
    [HttpGet("{email}", Name = "Get")]
    public IActionResult Get(string email)
    {
      try
      {
        var user = _userRepository.GetUserByEmail(email);
        return new OkObjectResult(user);
      }
      catch(Exception)
      {
        return new StatusCodeResult(500);
      }
      
    }


    [HttpPost]
    public IActionResult Post([FromBody] User user)
    {
      try
      {
        int count = _userRepository.CountUserByEmail(user.email_user);
        if (count == 0){  
          using (var scope = new TransactionScope())
          {          
            _userRepository.InsertUser(user);
            scope.Complete();
            return CreatedAtAction(nameof(Get), new { id = user.id_user }, user);
          }
        }else{
          return new OkObjectResult("Email already in use!");
        }
      }
      catch(Exception)
      {
        return new StatusCodeResult(500);
      }
      
    }

    [Authorize]
    [HttpPut("{id}", Name = "Put")]
    public IActionResult Put(int id, [FromBody] User user)
    {
      try
      {
        if (user != null)
        {
          using (var scope = new TransactionScope())
          {
            _userRepository.UpdateUser(user, id);
            scope.Complete();
            var userUpdated =_userRepository.GetUserById(id);
            return new OkObjectResult(userUpdated);
          }
        }
        return new NoContentResult();
      }
      catch(Exception)
      {
        return new StatusCodeResult(500);
      }
      
    }

    [Authorize]
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
      try
      {
        var userDeleted = _userRepository.GetUserById(id);
        _userRepository.DeleteUser(id);
        //return new OkResult();
        return new OkObjectResult(userDeleted);
      }
      catch(Exception)
      {
        return new StatusCodeResult(500);
      }

    }
  }
}