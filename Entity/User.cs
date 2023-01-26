namespace Entity;
public class User
{
    public string Nombres { get; set; }

    public string Apellidos { get; set; }

    public int Edad { get; set; }

    public string Username { get; set; } 

    public string Password { get; set; }

    public string Rol { get; set;} 

    public string Identificacion { get; set; }

    public int UserId { get; set; }

    public string Salt { get; set; }

}

public class DefaultUser
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Salt { get; set; }
}