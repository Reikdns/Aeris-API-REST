namespace API.Controllers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Entity;
using BLL;
using API.Models;
using API.Helper;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

[Route("[controller]")]
[ApiController]
public class UserController : Controller
{
    private readonly UserService _userService;
    public IConfiguration Configuration { get; }

    public UserController(IConfiguration configuration)
    {
        Configuration = configuration;
        string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
        _userService = new UserService(connectionString);
    }

    //[Authorize(Roles="Admin")]
    [HttpGet("get-users")]
    public ActionResult<List<UserViewModel>> GetUsers()
    {   
        var response = _userService.SearchAllUsers();

        if(response.Error)
        {
            return BadRequest(response.Message);
        }

        var users = response.Response.Select(p => new UserViewModel(p));

        return Ok(users);
    }

    //[Authorize(Roles="Admin")]
    [HttpPost("register-user")]
    public ActionResult<UserViewModel> RegisterUser(UserInputModel user){

        HashedPassword hashedPassword = HashHelper.Hash(user.Password);
        user.Password = hashedPassword.Password;
        user.Salt = hashedPassword.Salt;

        var response = _userService.SaveUser(MapUser(user));

        if(response.Error){
            return BadRequest(response.Message);
        }

        return Ok(response.Response);
    }

    [HttpPost("register-default-user")]
    public ActionResult<DefaultUserLoginModel> RegisterDefaultUser(DefaultUserLoginModel user)
    {
        HashedPassword hashedPassword = HashHelper.Hash(user.Password);
        user.Password = hashedPassword.Password;
        user.Salt = hashedPassword.Salt;

        var response = _userService.SaveUser(MapUser(user));

        if(response.Error)
        {
            return BadRequest(response.Message);
        }

        return Ok(response.Response);
    } 

<<<<<<< HEAD
    //[Authorize(Roles="Admin")]
=======
    [Authorize(Roles="Admin")]
>>>>>>> 52834fc7fe34d11e9c7a4d296b2049963287d5b1
    [HttpGet("search-by-key/{key}/{value}")]
    public ActionResult<UserViewModel> SearchByKey(string key, string value)
    {
        var response = _userService.SearchByKey(key, value);

        if(response.Error)
        {
            return BadRequest(response.Message);
        }

        return Ok(new UserViewModel(response.Response));
    }

    [HttpGet("identity")]
    public IActionResult GetIdentity()
    {
        var r = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier);
        return Ok(r == null ? "" : r.Value);
    }

    private User MapUser(UserInputModel user){
        return new User {
            Nombres = user.Nombres,
            Apellidos = user.Apellidos,
            Edad = user.Edad,
            Identificacion = user.Identificacion,
            Password = user.Password,
            Rol = user.Rol,
            Username = user.Username,
            Salt = user.Salt
        };
    }

    private DefaultUser MapUser(DefaultUserLoginModel user)
    {
        return new DefaultUser{
            Email = user.Email,
            Password = user.Password,
            Salt = user.Salt
        };
    }
}
