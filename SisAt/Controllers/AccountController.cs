using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SisAt.Models.ViewModel;
using SisAt.Models;
using AutoMapper;
using SisAt.Repository.Persistence.Interfaces;

namespace SisAt.Controllers;

public class AccountController : Controller
{
    public readonly IMapper _mapper;
    private readonly SignInManager<Usuario> _signInManager;
    private readonly UserManager<Usuario> _userManager;
    private readonly IMailService _emailSender;

    public AccountController(IMapper mapper, SignInManager<Usuario> signInManager, UserManager<Usuario> userManager, IMailService emailSender)
    {
        _mapper = mapper;
        _signInManager = signInManager;
        _userManager = userManager;
        _emailSender = emailSender;
    }

    public IActionResult Login(string returnUrl = null)
    {
        Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate, private";
        Response.Headers["Pragma"] = "no-cache";
        Response.Headers["Expires"] = "0";
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(UsuarioRequestDto login, string returnUrl = null)
    {
        try
        {
            returnUrl ??= Url.Content("~/AgendamentoHorarios");

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(login.Email);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, login.Senha, false, false);
                    if (result.Succeeded)
                    {
                        ViewBag.NomeUsuario = user.Nome;
                        return RedirectToLocal(returnUrl);
                    }
                }
                TempData["Error"] = "Login ou senha inválidos.";
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
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RegistrarUsuario(RegistrarUsuarioRequestDto model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                if (model.Senha != model.ConfirmarSenha)
                {
                    TempData["Error"] = "As senhas não coincidem.";
                    return View(model);
                }

                var user = new Usuario { UserName = model.Email, Email = model.Email, Nome = model.Nome };
                var result = await _userManager.CreateAsync(user, model.Senha);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("Index", "AgendamentoHorarios");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        TempData["Error"] = error.Description;
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(model);
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro {ex.Message}");
        }
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();

        Response.Cookies.Append(".AspNetCore.Identity.Application", "", new CookieOptions
        {
            Expires = DateTime.UtcNow.AddDays(-1),
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict
        });
        return RedirectToAction(nameof(AccountController.Login), "Account");
    }

    private IActionResult RedirectToLocal(string returnUrl)
    {
        if (Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }
        return RedirectToAction(nameof(AccountController.Login), "Account");
    }

    public IActionResult AcessoNegado()
    {
        return View();
    }

    public IActionResult RecuperarSenha()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RecuperarSenha(RecuperarSenhaViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetLink = Url.Action("RedefinirSenha", "Account", new { token = token, email = model.Email }, Request.Scheme);

                await _emailSender.SendMailAsync(user.Nome, model.Email, "Redefinir Senha",
                    $"Clique aqui para redefinir sua senha: {resetLink}");

                TempData["Sucesso"] = $"Um link de redefinição de senha foi enviado para o e-mail: {model.Email}.";
                return View();
            }

            TempData["Error"] = $"Não foi encontrado um usuário com o e-mail: {model.Email}.";
            return View();
        }
        return View(model);
    }

    [HttpGet]
    public IActionResult RedefinirSenha(string token, string email)
    {
        var model = new RedefinirSenhaViewModel { Token = token, Email = email };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RedefinirSenha(RedefinirSenhaViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user != null)
            {
                var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Senha);
                if (result.Succeeded)
                {
                    TempData["Sucesso"] = "Senha alterada com sucesso.";
                    return RedirectToAction("Login", "Account");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
        }
        return View(model);
    }
}
