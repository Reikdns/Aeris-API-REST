namespace API;

using Microsoft.AspNetCore.Mvc;
using System;
using Entity;
using API.Models;
using API.Helper;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using BLL;
using System.Text;

public class AuthenticationHelper
{
    public static string CreateToken(LoginViewModel systemUser, DefaultUser user, string rol, string secretKey)
    {
        if (HashHelper.CheckHash(systemUser.Password, user.Password, user.Salt))
        {
            var key = Encoding.ASCII.GetBytes(secretKey);

            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, systemUser.Email));

            claims.AddClaim(new Claim(ClaimTypes.Role, rol));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var createdToken = tokenHandler.CreateToken(tokenDescriptor);

            string bearer_token = tokenHandler.WriteToken(createdToken);
            return bearer_token;
        }
        else
        {
            return String.Empty;
        }

    }
}