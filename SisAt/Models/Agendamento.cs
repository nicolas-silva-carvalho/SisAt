using System.ComponentModel.DataAnnotations;

namespace SisAt.Models
{
    public class Agendamento
    {
        public int Id { get; set; }
        public string CpfCnpj { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public int CadastroDeHorarioId { get; set; }
        public int ServicoId { get; set; }
        public DateTime DataMarcada { get; set; }
        public string Hora { get; set; }
        public string? Protocolo { get; set; }
        public bool? ConfirmarAgendamento { get; set; }
    }
}
