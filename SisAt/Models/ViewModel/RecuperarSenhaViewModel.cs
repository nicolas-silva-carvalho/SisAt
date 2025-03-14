using System.ComponentModel.DataAnnotations;

namespace SisAt.Models.ViewModel
{
    public class RecuperarSenhaViewModel
    {
        [Required(ErrorMessage = "É obrigatório informar um e-mail.")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
