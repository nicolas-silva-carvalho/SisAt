using System.ComponentModel.DataAnnotations;

namespace SisAt.Models
{
    public class ConfirmarAgendamentoViewModel
    {
        public string? Protocolo { get; set; }
        public string? CPF { get; set; }
        public List<AgendamentoPesquisa> Agendamentos { get; set; }
        public bool PesquisaRealizada { get; set; }
    }
}
