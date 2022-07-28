using System.ComponentModel.DataAnnotations;
namespace API.Models
{
    public class LoginModel
    {
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage="Es necesario proporcionar el nombre de usuario.")]
        public string Username { get; set; }
        [Required(ErrorMessage="Es necesario proporcional la clave del ususario.")]
        public string Password { get; set; }

    }
}
