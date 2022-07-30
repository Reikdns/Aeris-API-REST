﻿using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using API.Models;
using API.Helper;
using BLL;
using Entity;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : Controller
    {
        public IConfiguration Configuration { get; }
        public readonly UserService _userService;

        public LoginController(IConfiguration configuration)
        {
            Configuration = configuration;
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            _userService = new UserService(connectionString);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel systemUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }

            var response = _userService.SearchByKey("username", systemUser.Username);

            if(response.Error)
            {
                return NotFound(ErrorHelper.Response(404, "Usuario no encontrado."));
            }

            var secretKey = Configuration.GetValue<string>("SecretKey");
            
            string bearerToken = AuthenticationHelper.CreateToken(systemUser, response.Response, secretKey);

            if (bearerToken.Equals(String.Empty))
            {
                return Forbid(); 
            }
            else
            {
                return Ok(bearerToken);
            }
        }
    }
}
