using System.ComponentModel.DataAnnotations;

namespace SisAt.Models
{
    public class AgendamentoViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Selecionar o serviço é obrigatório.")]
        public int id { get; set; }
        //public string Local { get; set; }
        [Required(ErrorMessage = "Informar o CPF ou CNPJ é obrigatório")]
        public string CpfCnpj { get; set; }
        [Required(ErrorMessage = "Informar o nome é obrigatório")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Informar o e-mail é obrigatório")]
        public string Email { get; set; }
    }
}
