using Microsoft.EntityFrameworkCore;
using SisAt.DataBase;
using SisAt.Models;
using SisAt.Repository.Persistence.Interfaces;

namespace SisAt.Repository.Persistence
{
    public class AgendamentoPersistence : IAgendamentoPersistence
    {
        public readonly Context _context;
        private readonly ICadastroDeHorariosPersistence _cadastroDeHorarios;

        public AgendamentoPersistence(Context context, ICadastroDeHorariosPersistence cadastroDeHorarios)
        {
            _context = context;
            _cadastroDeHorarios = cadastroDeHorarios;
        }

        public Task<Agendamento> AtualizarAgendamentoAsync(int agendamentoId, Agendamento agendamento)
        {
            throw new NotImplementedException();
        }

        public async Task<Agendamento> CadastrarAgendamentoAsync(Agendamento agendamento)
        {
            try
            {
                
                _context.Agendamentos.Add(agendamento);
                await _context.SaveChangesAsync();
                agendamento.Protocolo = $"{agendamento.DataMarcada.Day}" + $"{agendamento.DataMarcada.Month}" + $"{agendamento.DataMarcada.Year}" + $"{agendamento.ServicoId}" + $"{agendamento.Id}";
                _context.Agendamentos.Update(agendamento);
                await _context.SaveChangesAsync();
                var horario = await _cadastroDeHorarios.BuscarCadastroDeHorarioPorIdAsync(agendamento.CadastroDeHorarioId);
                horario.Marcado = true;
                await _cadastroDeHorarios.AtualizarCadastroDeHorarioPorIdAsync(horario.Id, horario);
                return await PegarAtendimentoPorIdAsync(agendamento.Id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task<bool> ConfirmarAgendamentoAsync(int agendamentId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeletarAgendamentoAsync(int agendamentoId)
        {
            throw new NotImplementedException();
        }

        public async Task<Agendamento> PegarAtendimentoPorIdAsync(int agendamentoId)
        {
            try
            {
                IQueryable<Agendamento> query = _context.Agendamentos.Where(x => x.Id == agendamentoId);

                if (query == null) return null;

                return await query.AsNoTracking().FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        public Task<List<Agendamento>> PegarTodosOsAgendamentoAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<Agendamento>> PegarAgendamentosPorProtocoloECPF(string protocolo, string cpf)
        {
            try
            {
                var agendamentos = _context.Agendamentos.Where(x => x.Protocolo == protocolo || x.CpfCnpj == cpf).Include(x => x.CadastroDeHorarios).Order();

                return await agendamentos.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
