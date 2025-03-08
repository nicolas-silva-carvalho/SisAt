using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SisAt.API;
using SisAt.Filtros;
using SisAt.Models;
using SisAt.Repository.Persistence;
using SisAt.Repository.Persistence.Interfaces;
using System.Text.RegularExpressions;

namespace SisAt.Controllers;
[FiltroLimparCache]
public class AgendamentoController : Controller
{
    public readonly IImportacaoAPIService _importacao;
    public readonly IMapper _mapper;
    public readonly IAgendamentoPersistence _agendamento;
    public readonly ICadastroDeHorariosPersistence _cadastro;
    public readonly IMailService _mailService;
    public AgendamentoController(IImportacaoAPIService importacao, IMapper mapper, IAgendamentoPersistence agendamento, ICadastroDeHorariosPersistence cadastro, IMailService mailService)
    {
        _importacao = importacao;
        _mapper = mapper;
        _agendamento = agendamento;
        _cadastro = cadastro;
        _mailService = mailService;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";
            ViewBag.MenuAtivo = "Inicio";
            var servicos = await _importacao.ServicosApiResponse();
            var servicosMap = _mapper.Map<List<ServicoViewModel>>(servicos.dados);
            ViewBag.Servicos = new SelectList(servicosMap, "id", "nome");

            
            if (TempData["Agendamento"] != null)
            {
                AgendamentoViewModel agendamentoViewModel = new AgendamentoViewModel();
                agendamentoViewModel.AgendamentoReturnViewl = JsonConvert.DeserializeObject<AgendamentoReturnViewl>(TempData["Agendamento"].ToString());

                return View(agendamentoViewModel);
            }

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
                    var cpf = Regex.Replace(agendamento.CpfCnpj, @"[^\d]", "");
                    agendamento.CpfCnpj = cpf;
                    var horario = JsonConvert.DeserializeObject<List<HorarioViewModel>>(horarioChecked);
                    var serv = await _importacao.ServicosApiResponse(agendamento.Id);

                    Agendamento ag = new Agendamento()
                    {
                        CpfCnpj = agendamento.CpfCnpj,
                        Nome = agendamento.Nome,
                        Email = agendamento.Email,
                        ServicoId = agendamento.Id,
                        ServicoNome = serv.nome
                    };

                    foreach (var item in horario)
                    {
                        ag.Hora = item.Hora;
                        ag.DataMarcada = Convert.ToDateTime(item.Data);
                        ag.CadastroDeHorarioId = item.Id;
                    }

                    var cadastro = await _cadastro.BuscarCadastroDeHorarioPorIdAsync(ag.CadastroDeHorarioId);

                    if (cadastro == null)
                    {
                        var servicosApi = await _importacao.ServicosApiResponse();
                        var servicosMappping = _mapper.Map<List<ServicoViewModel>>(servicosApi.dados);
                        ViewBag.Servicos = new SelectList(servicosMappping, "id", "nome");
                        TempData["Error"] = "Horário cadastrado não encontrado.";
                        return View(agendamento);
                    }
                    
                    ag.CadastroDeHorarios = cadastro;
                    var agendamentoRequest = await _agendamento.CadastrarAgendamentoAsync(ag);
                    
                    if (ag.Email != null)
                    {
                        var body = Template(agendamentoRequest.Protocolo, agendamentoRequest.Nome, agendamentoRequest.Email, agendamentoRequest.DataMarcada.ToString("dd/MM/yyyy"), agendamentoRequest.Hora, agendamentoRequest.ServicoNome);
                        await _mailService.SendMailAsync(agendamentoRequest.Nome, agendamentoRequest.Email, "CADASTRO DE AGENDAMENTO - PROTOCOLO DE AGENDAMENTO: " + $"{agendamentoRequest.Protocolo}", body);
                    }

                    TempData["Sucesso"] = "Agendamento cadastrado com sucesso.";

                    AgendamentoReturnViewl agendamentoReturnViewl = new AgendamentoReturnViewl()
                    {
                        Nome = agendamentoRequest.Nome,
                        Hora = agendamentoRequest.Hora,
                        DataMarcada = agendamentoRequest.DataMarcada,
                        Protocolo = agendamentoRequest.Protocolo,
                        ServicoNome = agendamentoRequest.ServicoNome
                    };

                    TempData["Agendamento"] = JsonConvert.SerializeObject(agendamentoReturnViewl);
                    return RedirectToAction("Index", "Agendamento");
                }
            }

            if (!ModelState.IsValid)
            {
                TempData["Calendario"] = true;
                var servicosApi = await _importacao.ServicosApiResponse();
                var servicosApiMap = _mapper.Map<List<ServicoViewModel>>(servicosApi.dados);
                ViewBag.Servicos = new SelectList(servicosApiMap, "id", "nome");

                return View(agendamento);
            }

            TempData["Info"] = "Por favor, selecione um horário para o agendamento."; 
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
            throw new Exception(ex.Message);
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

    public string Template(string protocolo, string nome, string email, string data, string hora, string servico)
    {
        string mailBody = @$"
        <div>
            <table border='0' cellpadding='0' cellspacing='0' width='555' style='background-color:#e4e4e4;margin-left:auto;margin-right:auto'>
                <tbody>
                    <tr>
                        <td>
                            <table border='0' cellpadding='0' cellspacing='0' width='100%'>
                                <tbody>
                                    <tr>
                                        <td align='center' valign='top' colspan='4'><img alt='barra_top' tabindex='0' style='width: 518px; height: 78px'>
                                            <div class='a6S' dir='ltr' style='opacity: 0.01; left: 507px; top: 102px;'>
                                                <div id=':481' class='T-I J-J5-Ji aQv T-I-ax7 L3 a5q' role='button' tabindex='0' aria-label='' data-tooltip-class='a1V' data-tooltip=''>
                                                    <div class='aSK J-J5-Ji aYr'>
                                                    </div>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <table cellpadding='0' cellspacing='0' width='555' border='0'>
                                <tbody>
                                    <tr>
                                        <td width='15'>&nbsp;</td>
                                        <td style='background-color:#ffffff'>
                                            <table cellpadding='0' cellspacing='0' width='100%' border='0'>
                                                <tbody>
                                                    <tr>
                                                        <td style='padding-top:15px;padding-bottom:15px;padding-left:30px;font-size:14px;font-family:Tahoma,Arial,Sans-Serif' class='style1'><H2>Protocolo Nº: {protocolo} </H2><tr> 
                                                        <td align='center' valign='top'>__________________________________________________________________</td></tr></td><td>&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td style='padding-left:30px;padding-bottom:20px;font-size:13px;font-family:Tahoma,Arial,Sans-Serif'><strong>Tipo Serviço:</strong> {servico.ToUpper()}</td>
                                                    </tr>
                                                    <tr>
                                                        <td style='padding-left:30px;padding-bottom:20px;font-size:13px;font-family:Tahoma,Arial,Sans-Serif'><strong>Solicitante:</strong> {nome.ToUpper()}</td>
                                                    </tr>
                                                    <tr>
                                                        <td style='padding-left:30px;font-size:13px;font-family:Tahoma,Arial,Sans-Serif; padding-bottom: 20px;'><strong>Data Agendamento:</strong> {data}</td>
                                                    </tr>
                                                    <tr>
                                                        <td style='padding-left:30px;font-size:13px;font-family:Tahoma,Arial,Sans-Serif; padding-bottom: 20px;'><strong>Horário do Agendamento:</strong> {hora}</td>
                                                    </tr>
                                                    <tr>
                                                        <td style='padding-left:30px;font-size:13px;font-family:Tahoma,Arial,Sans-Serif;padding-bottom:20px'><strong>e-mail:</strong> {email.ToUpper()}</td>
                                                    </tr>
                                                    <tr>
                                                        <td style='padding-left:30px;margin-top:10px;font-size:13px;font-family:Tahoma,Arial,Sans-Serif'><H5>Observações: </H5>  Por favor, chegar com antecedência no horário agendado e dirigir-se ao guichê de atendimento.</td>
                                                    </tr>
                                                    <tr>
                                                        <td align='center'>__________________________________________________________________</td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td style='padding-left:30px;padding-right:35px;margin-top:5px;font-size:11px;font-family:Tahoma,Arial,Sans-Serif' 
                                                            <p> *Este é um email que foi enviado automaticamente, por favor não responder. </p>
                                                            <p>As informações existentes nessa mensagem são de uso restrito, sendo seu sigilo protegido por lei.</p> Caso não seja   destinatário, saiba que a leitura, divulgação ou cópia são proibidas. Favor apagar as informações e notificar o remetente. O uso impróprio será tratado conforme as normas da legislação em vigor.<p>  
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    <td width='15'>&nbsp;</td>
                                </tr>
                            <tr>
                        <td colspan='3' height='15'>&nbsp;</td>
                    </tr>       
                </tbody>
            </table>
            </td></tr>
            </tbody>
            </table>
            <div class='yj6qo'></div><div class='adL'></div></div>";
        return mailBody;
    }
}
