using System.ComponentModel.DataAnnotations;

namespace SisAt.Models
{
    public class ConfirmarAgendamentoViewModel
    {
        [Required(ErrorMessage = "É obrigatório confirmar a senha.")]
        public string Protocolo { get; set; }
    }
}
