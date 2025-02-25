using System.ComponentModel.DataAnnotations;

namespace SisAt.Models
{
    public class RegistrarUsuarioRequestDto
    {
        [Required(ErrorMessage = "Nome de usuário é obrigatório.")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Email do usuário é obrigatório.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Senha de usuário é obrigatório.")]
        public string Senha { get; set; }
        [Required(ErrorMessage = "Confirmar a senha é obrigatório.")]
        public string ConfirmarSenha { get; set; }
    }
}
