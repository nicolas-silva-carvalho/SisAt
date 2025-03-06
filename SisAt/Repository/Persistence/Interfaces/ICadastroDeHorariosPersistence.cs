using SisAt.Models;

namespace SisAt.Repository.Persistence.Interfaces
{
    public interface ICadastroDeHorariosPersistence
    {
        Task<List<Horarios>> BuscarHorariosAsync(int servicoId);
        Task<List<Horarios>> BuscarHorariosAsync(DateTime data, int servicoId);
        Task<Horarios> CadastrarHorarioAsync(Horarios horarios);
        Task<CadastroDeHorarios> CadastrarDataEServicoAsync(CadastroDeHorarios cadastroDeHorarios);
        Task<List<CadastroDeHorarios>> BuscarHorariosDisponiveisPorServico(DateTime? data, int servicoId);
        Task<CadastroDeHorarios> BuscarCadastroDeHorarioPorIdAsync(int cadastroId);
        Task<CadastroDeHorarios> AtualizarCadastroDeHorarioPorIdAsync(int cadastroId, CadastroDeHorarios cadastroDeHorarios);
        Task<Senha> ConfirmarAgendamentoAsync(int agendamentoId);
    }
}
