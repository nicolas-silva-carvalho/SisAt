using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SisAt.API;
using SisAt.Models;
using SisAt.Repository.Persistence.Interfaces;

namespace SisAt.Controllers;
public class AgendamentoController : Controller
{
    public readonly IImportacaoAPIService _importacao;
    public readonly IMapper _mapper;
    public readonly IAgendamentoPersistence _agendamento;
    public readonly ICadastroDeHorariosPersistence _cadastro;
    public AgendamentoController(IImportacaoAPIService importacao, IMapper mapper, IAgendamentoPersistence agendamento, ICadastroDeHorariosPersistence cadastro)
    {
        _importacao = importacao;
        _mapper = mapper;
        _agendamento = agendamento;
        _cadastro = cadastro;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var servicos = await _importacao.ServicosApiResponse();
            var servicosMap = _mapper.Map<List<ServicoViewModel>>(servicos.dados);
            ViewBag.Servicos = new SelectList(servicosMap, "id", "nome");

            return View();
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Index(AgendamentoViewModel agendamento, string horarioChecked)
    {
        try
        {
            if (ModelState.IsValid && horarioChecked != "[]")
            {
                if (horarioChecked != null)
                {
                    var horario = JsonConvert.DeserializeObject<List<HorarioViewModel>>(horarioChecked);
                    if (horario.Count > 1)
                    {
                        var servicosApi = await _importacao.ServicosApiResponse();
                        var servicosMappping = _mapper.Map<List<ServicoViewModel>>(servicosApi.dados);
                        ViewBag.Servicos = new SelectList(servicosMappping, "id", "nome");
                        TempData["Error"] = "Selecione somente um horário para o agendamento.";
                        return View(agendamento);
                    }

                    Agendamento ag = new Agendamento()
                    {
                        CpfCnpj = agendamento.CpfCnpj,
                        Nome = agendamento.Nome,
                        Email = agendamento.Email,
                        ServicoId = agendamento.Id
                    };

                    foreach (var item in horario)
                    {
                        ag.Hora = item.Hora;
                        ag.DataMarcada = Convert.ToDateTime(item.Data);
                        ag.CadastroDeHorarioId = item.Id;
                    }

                    var agendamentoRequest = await _agendamento.CadastrarAgendamentoAsync(ag);
                    TempData["Sucesso"] = "Agendamento cadastrado com sucesso.";
                    return RedirectToAction("Index", "Agendamento");
                }
            }

            var servicos = await _importacao.ServicosApiResponse();
            var servicosMap = _mapper.Map<List<ServicoViewModel>>(servicos.dados);
            ViewBag.Servicos = new SelectList(servicosMap, "id", "nome");

            return View(agendamento);
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro {ex.Message}");
        }
    }

    public async Task<IActionResult> Servicos(int agendamentoId)
    {
        try
        {
            var servicos = await _importacao.ServicosApiResponse();
            var servicosMap = _mapper.Map<List<ServicoViewModel>>(servicos.dados);
            ViewBag.Servicos = new SelectList(servicosMap, "id", "nome");

            return View();
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    public async Task<IActionResult> DiasDisponiveisAsync(int id)
    {
        try
        {
            var dias = await _cadastro.BuscarHorariosDisponiveisPorServico(null, id);

            if (dias == null || dias.Count == 0)
            {
                return Json(new { success = false, message = "Nenhum horário encontrado para este serviço." });
            }

            return Json(new { success = true, dias = dias });
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<IActionResult> HorariosAsync(string data, int id)
    {
        try
        {
            var dataCon = Convert.ToDateTime(data);
            var horarios = await _cadastro.BuscarHorariosDisponiveisPorServico(dataCon, id);

            if (horarios == null || horarios.Count == 0)
            {
                return Json(new { success = false, message = "Nenhum horário encontrado para este serviço." });
            }

            return Json(new { success = true, horarios = horarios });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }
}
