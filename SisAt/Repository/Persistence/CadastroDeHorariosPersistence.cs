using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SisAt.API;
using SisAt.DataBase;
using SisAt.Models;
using SisAt.Repository.Persistence.Interfaces;

namespace SisAt.Repository.Persistence
{
    public class CadastroDeHorariosPersistence : ICadastroDeHorariosPersistence
    {
        public readonly Context _context;
        public readonly IImportacaoAPIService _importacao;
        public readonly IMapper _mapper;
        public CadastroDeHorariosPersistence(Context context, IImportacaoAPIService importacao, IMapper mapper)
        {
            _context = context;
            _importacao = importacao;
            _mapper = mapper;
        }

        public async Task<List<Horarios>> BuscarHorariosAsync(int servicoId)
        {
            try
            {
                var horarios = await _context.Horarios.ToListAsync();
                if (horarios == null) return null;

                return horarios;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Horarios>> BuscarHorariosAsync(DateTime data, int servicoId)
        {
            try
            {
                if (data.Day == DateTime.Now.Day)
                {
                    var hora = DateTime.Now.Hour.ToString("D2");
                    var minutos = DateTime.Now.Minute.ToString("D2");
                    string horaMinuto = $"{hora}:{minutos}";
                    var horarios = await _context.Horarios.ToListAsync();

                    if (horarios == null || !horarios.Any()) return null;
                    var horariosFiltrados = horarios.Where(x => string.Compare(x.Hora, horaMinuto) > 0).ToList();

                    var cadastroDiaAtual = await _context.CadastroDeHorarios
                        .Where(x => x.ServicoId == servicoId && x.DataCadastrada.Day == data.Day)
                        .ToListAsync();

                    var horariosCadastradosFiltroIds = new HashSet<int>(cadastroDiaAtual.Select(c => c.HoraId));

                    var horariosDisponivelsFiltrado = horariosFiltrados
                        .Select(h => new Horarios
                        {
                            Id = h.Id,
                            Hora = h.Hora,
                            Disponivel = !horariosCadastradosFiltroIds.Contains(h.Id)
                        })
                        .ToList();

                    return horariosDisponivelsFiltrado;
                }

                var horariosTotal = await _context.Horarios.ToListAsync();

                var cadastro = await _context.CadastroDeHorarios
                    .Where(x => x.ServicoId == servicoId && x.DataCadastrada.Day == data.Day)
                    .ToListAsync();

                var horariosCadastradosIds = new HashSet<int>(cadastro.Select(c => c.HoraId));

                var horariosDisponivelsParaOServico = horariosTotal
                    .Select(h => new Horarios
                    {
                        Id = h.Id,
                        Hora = h.Hora,
                        Disponivel = !horariosCadastradosIds.Contains(h.Id)
                    })
                    .ToList();

                return horariosDisponivelsParaOServico;

            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar horários: {ex.Message}");
            }
        }

        public async Task<Horarios> CadastrarHorarioAsync(Horarios horarios)
        {
            try
            {
                _context.Horarios.Add(horarios);
                await _context.SaveChangesAsync();
                return horarios;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CadastroDeHorarios> CadastrarDataEServicoAsync(CadastroDeHorarios cadastroDeHorarios)
        {
            try
            {
                _context.CadastroDeHorarios.Add(cadastroDeHorarios);
                await _context.SaveChangesAsync();
                return cadastroDeHorarios;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<CadastroDeHorarios>> BuscarHorariosDisponiveisPorServico(DateTime? data, int servicoId)
        {
            try
            {
                if (data == null)
                {
                    var dias = _context.CadastroDeHorarios.Where(x => x.ServicoId == servicoId && x.Marcado == false && x.DataCadastrada.Date >= DateTime.Now.Date).OrderBy(x => x.Id);

                    if (DateTime.Now.Date == dias.FirstOrDefault()?.DataCadastrada.Date) 
                    {
                        var hora = DateTime.Now.Hour.ToString("D2");
                        var minutos = DateTime.Now.Minute.ToString("D2");
                        string horaMinuto = $"{hora}:{minutos}";

                        dias = dias.Where(x => x.DataCadastrada.Date > DateTime.Now.Date || string.Compare(x.Hora, horaMinuto) > 0).OrderBy(x => x.Id);
                    }

                    return await dias.ToListAsync();
                }

                var cadastros = _context.CadastroDeHorarios.Where(x => x.ServicoId == servicoId && x.Marcado == false && x.DataCadastrada.Day == data.Value.Day).OrderBy(x => x.Id);
                var horaT = DateTime.Now.Hour.ToString("D2");
                var minutosT = DateTime.Now.Minute.ToString("D2");
                string horaMinutoT = $"{horaT}:{minutosT}";

                if (DateTime.Now.Date == cadastros.FirstOrDefault()?.DataCadastrada.Date)
                {
                    var hora = DateTime.Now.Hour.ToString("D2");
                    var minutos = DateTime.Now.Minute.ToString("D2");
                    string horaMinuto = $"{hora}:{minutos}";

                    cadastros = cadastros.Where(x => x.DataCadastrada.Date > DateTime.Now.Date || string.Compare(x.Hora, horaMinuto) > 0).OrderBy(x => x.Id);

                    return await cadastros.ToListAsync();
                }

                if (cadastros == null) return null;

                return await cadastros.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CadastroDeHorarios> BuscarCadastroDeHorarioPorIdAsync(int cadastroId)
        {
            try
            {
                var cadastro = await _context.CadastroDeHorarios.Where(x => x.Id == cadastroId).FirstOrDefaultAsync();

                return cadastro;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CadastroDeHorarios> AtualizarCadastroDeHorarioPorIdAsync(int cadastroId, CadastroDeHorarios cadastroDeHorarios)
        {
            try
            {
                var getCadastro = await BuscarCadastroDeHorarioPorIdAsync(cadastroId);

                if (getCadastro == null) return null;

                _context.Update(cadastroDeHorarios);
                await _context.SaveChangesAsync();

                return cadastroDeHorarios;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<SisAt.Models.Senha> ConfirmarAgendamentoAsync(string protocolo)
        {
            var agendamento = await _context.Agendamentos.Where(x => x.Protocolo == protocolo && x.ConfirmarAgendamento == null).Include(x => x.CadastroDeHorarios).FirstOrDefaultAsync();

            if (agendamento == null) return null;

            var hora = DateTime.Now.Hour.ToString("D2");
            var minutos = DateTime.Now.Minute.ToString("D2");
            string horaMinuto = $"{hora}:{minutos}";

            if (string.Compare(agendamento.CadastroDeHorarios.Hora, horaMinuto) >= 0)
            {
                agendamento.ConfirmarAgendamento = true;
                _context.Agendamentos.Update(agendamento);
                await _context.SaveChangesAsync();
                var response = await _importacao.CriacaoDeSenha(agendamento.ServicoId, agendamento.Nome);
                var senha = _mapper.Map<SisAt.Models.Senha>(response.dados.senha);
                return senha;
            }

            return null;
        }
    }
}
