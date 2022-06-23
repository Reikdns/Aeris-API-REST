
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

[Route("api/[controller]")]
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

    [HttpGet("get-users")]
    public ActionResult<UserViewModel> GetUsers()
    {   
        var response = _userService.SearchAllUsers();

        if(response.Error)
        {
            return BadRequest(response.Message);
        }

        var users = response.Users.Select(p => new UserViewModel(p));

        return Ok(users);
    }

    [HttpPost("register-users")]
    public ActionResult<UserViewModel> RegisterUser(UserInputModel user){
        var response = _userService.SaveUser(MapUser(user));

        if(response.User is null){
            return BadRequest(response.Message);
        }

        return Ok(response.User);
    }

    [HttpPost("search-user/{identification}")]
    public ActionResult<UserViewModel> SearchUser(string identification)
    {
        var response = _userService.SearchById(identification);

        if(response.Error)
        {
            return BadRequest(response.Message);
        }

        return Ok(response.User);
    }


    private User MapUser(UserInputModel user){
        return new User {
            Nombres = user.Nombres,
            Apellidos = user.Apellidos,
            Edad = user.Edad,
            Identificacion = user.Identificacion,
            Password = user.Password,
            Rol = user.Rol,
            Username = user.Username
        };
    }

}
