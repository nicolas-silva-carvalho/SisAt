using SisAt.Models;

namespace SisAt.Repository.Persistence.Interfaces
{
    public interface IAgendamentoPersistence
    {
        Task<Agendamento> PegarAtendimentoPorIdAsync(int agendamentoId);
        Task<List<Agendamento>> PegarTodosOsAgendamentoAsync();
        Task<Agendamento> CadastrarAgendamentoAsync(Agendamento agendamento);
        Task<Agendamento> AtualizarAgendamentoAsync(int agendamentoId, Agendamento agendamento);
        Task<bool> DeletarAgendamentoAsync(int agendamentoId);
        Task<bool> ConfirmarAgendamentoAsync(int agendamentId);
        Task<List<Agendamento>> PegarAgendamentosPorProtocoloECPF(string protocolo, string cpf);
        Task<List<Calendario>> PegarTodosOsAgendamentoCalendarioAsync(int mes);
        Task<Calendario> AdicionarCalendarioAsync(Calendario calendario);
    }
}
