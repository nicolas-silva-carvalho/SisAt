using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SisAt.Helper;
using SisAt.Models;
using SisAt.Models.ViewModel;
using SisAt.Repository.Persistence.Interfaces;
using SisAt.Sessao;

namespace SisAt.Controllers;

public class HomeController : Controller
{
    public readonly ISessaoFactory _sessao;
    public readonly IUsuarioPersistence _usuario;
    public readonly IMapper _mapper;

    public HomeController(ISessaoFactory sessao, IUsuarioPersistence usuario, IMapper mapper)
    {
        _sessao = sessao;
        _usuario = usuario;
        _mapper = mapper;
    }

    public IActionResult Index()
    {
        Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate, private";
        Response.Headers["Pragma"] = "no-cache";
        Response.Headers["Expires"] = "0";
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(UsuarioRequestDto login)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var usuarioBanco = await _usuario.BuscarUsuarioPorEmailESenhaAsync(login.Email, login.Senha.GerarHash());
                
                if (usuarioBanco != null)
                {
                    if (usuarioBanco.SenhaValida(login.Senha))
                    {
                        _sessao.CriarSessao(usuarioBanco);
                        return RedirectToAction("Index", "AgendamentoHorarios");
                    }
                }
                TempData["Error"] = "Usuário ou senha inválidas.";
            }

            return View(login);
        }
        catch (Exception ex)
        {

            throw new Exception($"Erro {ex.Message}");
        }
    }

    public IActionResult RegistrarUsuario()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> RegistrarUsuario(RegistrarUsuarioRequestDto registrarUsuario)
    {
        try
        {
            if (ModelState.IsValid)
            {
                if (registrarUsuario.Senha == registrarUsuario.ConfirmarSenha)
                {
                    var usuario = _mapper.Map<Usuario>(registrarUsuario);
                    await _usuario.CriarUsuarioAsync(usuario);
                    TempData["Sucesso"] = "Usuário cadastrado com sucesso";
                    return RedirectToAction("Index", "Home");
                }

                TempData["Error"] = "Senhas diferentes. Por favor confirma a senha.";
                return View(registrarUsuario);
            }

            return View(registrarUsuario);

        }
        catch (Exception ex)
        {

            throw new Exception($"Erro {ex.Message}");
        }
    }

    public IActionResult Sair()
    {
        _sessao.RemoverSessaoPorId();
        return RedirectToAction("Index", "Home");
    }
}
