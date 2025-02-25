using System.ComponentModel.DataAnnotations;

namespace SisAt.Models
{
    public class UsuarioRequestDto
    {
        [Required(ErrorMessage = "Email obrigatório.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Senha obrigatória.")]
        public string Senha { get; set; }
    }
}
