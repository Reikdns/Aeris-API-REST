using System.ComponentModel.DataAnnotations;
namespace API.Models
{
    public class LoginModel
    {
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage="Es necesario proporcionar el nombre de usuario.")]
        public string Email { get; set; }
        [Required(ErrorMessage="Es necesario proporcional la clave del ususario.")]
        public string Password { get; set; }

    }

    public class LoginResponseModel
    {
        public string Token { get; set; }
        public string Rol { get; set; }     

        public LoginResponseModel(string token, string rol)
        {
            Token = token;
            Rol = rol;
        }
    }
}
