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
[Authorize]
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

    [Authorize(Roles="Admin")]
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

    [Authorize(Roles="Admin")]
    [HttpPost("register-user")]
    public ActionResult<UserViewModel> RegisterUser(UserInputModel user){

        HashedPassword hashedPassword = HashHelper.Hash(user.Password);
        user.Password = hashedPassword.Password;
        user.Salt = hashedPassword.Salt;

        var response = _userService.SaveUserPersonalData(MapUser(user));

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

    [Authorize(Roles="Admin")]
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
        var nameIdentifier = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier);
        var rol = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Role);
        if(nameIdentifier == null || rol == null) {return Forbid();}
        IdentityModel identityModel = new IdentityModel(nameIdentifier.Value, rol.Value);
        return Ok(identityModel == null ? "" : identityModel);
    }

    private User MapUser(UserInputModel user){
        return new User {
            Nombres = user.Nombres,
            Apellidos = user.Apellidos,
            Edad = user.Edad,
            Identificacion = user.Identificacion,
            Rol = user.Rol,
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
