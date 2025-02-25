using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SisAt.API;
using SisAt.Filtros;
using SisAt.Models;
using SisAt.Repository.Persistence.Interfaces;

namespace SisAt.Controllers;
[FiltroUsuarioLogado]
public class AgendamentoHorariosController : Controller
{
    public readonly IMapper _mapper;
    public readonly IImportacaoAPIService _importacao;
    public readonly ICadastroDeHorariosPersistence _cadastro;
    public AgendamentoHorariosController(IMapper mapper, IImportacaoAPIService importacao, ICadastroDeHorariosPersistence cadastro)
    {
        _mapper = mapper;
        _importacao = importacao;
        _cadastro = cadastro;
    }

    public IActionResult Index()
    {
        ViewBag.MenuAtivo = "Inicio";
        return View();
    }

    public async Task<IActionResult> CadastroDeHorarios()
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

    [HttpPost]
    public async Task<IActionResult> CadastroDeHorarios(CadastroDeHorariosViewModel cadastros, string horariosChecked)
    {
        try
        {
            if (ModelState.IsValid && horariosChecked != "[]" )
            {
                if (horariosChecked != null)
                {
                    var horarios = JsonConvert.DeserializeObject<List<Horarios>>(horariosChecked);

                    foreach (var hora in horarios)
                    {
                        CadastroDeHorarios cadastroDeHorarios = new CadastroDeHorarios()
                        {
                            ServicoId = cadastros.id,
                            DataCadastrada = Convert.ToDateTime(cadastros.DataCadastrada),
                            Hora = hora.Hora,
                            HoraId = hora.Id
                        };

                        await _cadastro.CadastrarDataEServicoAsync(cadastroDeHorarios);
                    }
                }
                
                TempData["Sucesso"] = "Horários para o serviço cadastrada com sucesso.";
                return RedirectToAction("Index", "AgendamentoHorarios");
            }

            if(ModelState.IsValid && horariosChecked == "[]")
            {
                TempData["Error"] = "Selecione pelo menos um horário para o serviço";
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
}
