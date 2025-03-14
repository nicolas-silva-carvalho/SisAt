using System.ComponentModel.DataAnnotations;

namespace SisAt.Models.ViewModel
{
    public class RedefinirSenhaViewModel
    {
        public string Token { get; set; }
        public string Email { get; set; }

        [Required(ErrorMessage = "É obrigatório informar a nova senha.")]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Senha { get; set; }

        [Required(ErrorMessage = "É obrigatório confirmar a nova senha.")]
        [DataType(DataType.Password)]
        [Compare("Senha", ErrorMessage = "As senhas não coincidem.")]
        public string ConfirmarSenha { get; set; }
    }
}
