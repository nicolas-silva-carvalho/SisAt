using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SisAt.API;
using SisAt.Models;
using SisAt.Models.ViewModel;
using SisAt.Repository.Persistence.Interfaces;

namespace SisAt.Controllers;
[Authorize]
public class AgendamentoHorariosController : Controller
{
    public readonly IMapper _mapper;
    public readonly IImportacaoAPIService _importacao;
    public readonly ICadastroDeHorariosPersistence _cadastro;
    public readonly IAgendamentoPersistence _agendamento;
    public AgendamentoHorariosController(IMapper mapper, IImportacaoAPIService importacao, ICadastroDeHorariosPersistence cadastro, IAgendamentoPersistence agendamento)
    {
        _mapper = mapper;
        _importacao = importacao;
        _cadastro = cadastro;
        _agendamento = agendamento;
    }

    public async Task<IActionResult> Index()
    {
        if (!User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
        }

        ViewBag.MenuAtivo = "Inicio";

        var calendario = await _agendamento.PegarTodosOsAgendamentoCalendarioAsync(DateTime.Now.Month);

        if (calendario.Count > 0 || calendario != null)
        {
            var calendarioMap = _mapper.Map<List<CalendarioViewlModel>>(calendario);
            var serialize = JsonConvert.SerializeObject(calendarioMap);
            ViewBag.Calendario = serialize;
            return View();
        }

        return View();
    }

    public async Task<IActionResult> CadastroDeHorarios()
    {
        try
        {
            ViewBag.MenuAtivo = "Horario";
            ViewBag.ActivePage = "CadastroHorario";
            var servicos = await _importacao.ServicosApiResponse();
            var servicosMap = _mapper.Map<List<ServicoViewModel>>(servicos.dados);
            ViewBag.Servicos = new SelectList(servicosMap, "id", "nome");

            return View();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CadastroDeHorarios(CadastroDeHorariosViewModel cadastros, string horariosChecked)
    {
        try
        {
            ViewBag.MenuAtivo = "Horario";
            ViewBag.ActivePage = "CadastroHorario";

            if (ModelState.IsValid && horariosChecked != "[]" )
            {
                if (horariosChecked != null)
                {
                    var horarios = JsonConvert.DeserializeObject<List<HorarioViewModel>>(horariosChecked);

                    foreach (var hora in horarios)
                    {
                        CadastroDeHorarios cadastroDeHorarios = new CadastroDeHorarios()
                        {
                            ServicoId = cadastros.id,
                            DataCadastrada = Convert.ToDateTime(hora.Data),
                            Hora = hora.Hora,
                            HoraId = hora.Id
                        };

                        await _cadastro.CadastrarDataEServicoAsync(cadastroDeHorarios);
                    }
                }
                
                TempData["Sucesso"] = "Horários para o serviço cadastrada com sucesso.";
                return RedirectToAction("Index", "AgendamentoHorarios");
            }

            if (ModelState.IsValid && horariosChecked == "[]")
            {
                TempData["Info"] = "Selecione pelo menos um horário para o serviço";
                var servicosRetorno = await _importacao.ServicosApiResponse();
                var servicosMapping = _mapper.Map<List<ServicoViewModel>>(servicosRetorno.dados);
                ViewBag.Servicos = new SelectList(servicosMapping, "id", "nome");
                return View(cadastros);
            }

            var servicos = await _importacao.ServicosApiResponse();
            var servicosMap = _mapper.Map<List<ServicoViewModel>>(servicos.dados);
            ViewBag.Servicos = new SelectList(servicosMap, "id", "nome");

            return View(cadastros);
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
            var horarios = await _cadastro.BuscarHorariosAsync(dataCon, id);

            if (horarios == null || horarios.Count == 0)
            {
                return Json(new { success = false, message = "Nenhum horário encontrado." });
            }

            return Json(new { success = true, horarios = horarios });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    public IActionResult ConfirmarAgendamento()
    {
        ViewBag.MenuAtivo = "Confirmar";

        if (TempData["Senha"] != null)
        {
            ConfirmarAgendamentoViewModel confirmarAgendamentoViewModel = new ConfirmarAgendamentoViewModel();
            confirmarAgendamentoViewModel.SenhaViewlModel = JsonConvert
                .DeserializeObject<SenhaViewlModel>(TempData["Senha"].ToString());

            return View(confirmarAgendamentoViewModel);
        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ConfirmarAgendamento(ConfirmarAgendamentoViewModel confirmarAgendamentoViewModel)
    {
        try
        {
            ViewBag.MenuAtivo = "Confirmar";

            if (confirmarAgendamentoViewModel.CPF != null || confirmarAgendamentoViewModel.Protocolo != null)
            {
                var agendamento = await _agendamento.PegarAgendamentosPorProtocoloECPF(confirmarAgendamentoViewModel.Protocolo, confirmarAgendamentoViewModel.CPF);

                if (agendamento == null || agendamento.Count == 0)
                {
                    TempData["Error"] = $"Não foi encontrado nenhum agendamento para o CPF: {confirmarAgendamentoViewModel.CPF} ou Protocolo: {confirmarAgendamentoViewModel.Protocolo}";
                    return View(confirmarAgendamentoViewModel);
                }

                var agendamentoMap = _mapper.Map<List<AgendamentoPesquisa>>(agendamento);
                confirmarAgendamentoViewModel.PesquisaRealizada = true;
                confirmarAgendamentoViewModel.Agendamentos = agendamentoMap;

                return View(confirmarAgendamentoViewModel);
            }

            TempData["Info"] = "Preencha o Protocolo ou CPF do cadastro no agendamento.";
            return View();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<IActionResult> ConfirmarAgendamentoById(int agendamentoId)
    {
        try
        {
            var senha = await _cadastro.ConfirmarAgendamentoAsync(agendamentoId);

            if (senha == null)
            {
                TempData["Error"] = "Para confirmar um atendimento é necessário confirma-lo antes do horario agendado.";
                return RedirectToAction("ConfirmarAgendamento", "AgendamentoHorarios");
            }

            var senhaMap = _mapper.Map<SenhaViewlModel>(senha);
            TempData["Senha"] = JsonConvert.SerializeObject(senhaMap);
            return RedirectToAction("ConfirmarAgendamento");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
