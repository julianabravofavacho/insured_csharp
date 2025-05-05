using System.ComponentModel.DataAnnotations;

namespace WebApi_Coris.Models
{
    public class AuthenticationModel
    {
        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
        public required string email { get; set; }
        [Required(ErrorMessage = "A senha é obrigatória.")]
        public required string password { get; set; }
    }
}
