using System.ComponentModel.DataAnnotations;

namespace SisAt.Models.ViewModel
{
    public class RecuperarSenhaViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
