using System.ComponentModel.DataAnnotations;

namespace SisAt.Models
{
    public class AgendamentoViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Selecionar o serviço é obrigatório.")]
        public int id { get; set; }
        [Required(ErrorMessage = "Informar o CPF é obrigatório")]
        public string CpfCnpj { get; set; }
        [Required(ErrorMessage = "Informar o nome é obrigatório")]
        public string Nome { get; set; }
        public string? Email { get; set; }
    }
}
