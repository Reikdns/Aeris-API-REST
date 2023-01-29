﻿namespace Entity;
public class User
{
    public string? Nombres { get; set; }

    public string? Apellidos { get; set; }

    public int? Edad { get; set; }

    public string Rol { get; set;} 

    public string? Identificacion { get; set; }

    public string Email { get; set; }

    public int UserId { get; set; }


}

public class DefaultUser
{
    public int UserId { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Salt { get; set; }
}

public class IdentityModel{
    public string Email { get; set; }
    public string Rol { get; set; }

    public IdentityModel(string email, string rol)
    {
        Email = email;
        Rol = rol;
    }
}

