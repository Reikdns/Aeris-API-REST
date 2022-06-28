using Domain.Entities;

namespace API.Models;

public class UserInputModel : UserViewModel
{
    public string Password { get; set; }

    public UserInputModel()
    {
    
    }
}

public class UserViewModel 
{
    public string Nombres { get; set; }

    public string Apellidos { get; set; }

    public int Edad { get; set; }

    public string Username { get; set; } 

    public string Rol { get; set;} 

    public string Identificacion { get; set; }

    public int UserId { get; set; }

    public UserViewModel()
    {
        
    }

    public UserViewModel(User user)
    {
        Nombres = user.Nombres;
        Apellidos = user.Apellidos;
        Edad = user.Edad; 
        Username = user.Username;
        Rol = user.Rol;
        Identificacion = user.Identificacion;
        UserId = user.UserId;
    }
} 