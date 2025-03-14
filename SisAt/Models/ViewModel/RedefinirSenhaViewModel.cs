using System.ComponentModel.DataAnnotations;

namespace SisAt.Models.ViewModel
{
    public class RedefinirSenhaViewModel
    {
        public string Token { get; set; }
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Senha { get; set; }

        [DataType(DataType.Password)]
        [Compare("Senha", ErrorMessage = "As senhas não coincidem.")]
        public string ConfirmarSenha { get; set; }
    }
}
