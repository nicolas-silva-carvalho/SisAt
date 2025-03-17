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
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "AgendamentoHorarios");
        }

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
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "AgendamentoHorarios");
        }

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

    public IActionResult RecuperarSenha()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "AgendamentoHorarios");
        }

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

                var body = Template(resetLink);
                await _emailSender.SendMailAsync(user.Nome, model.Email, "Redefinir Senha", Template(resetLink));

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
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "AgendamentoHorarios");
        }

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

    public string Template(string url)
    {
        string mailBody = @$"
        <style>
            .button-50 {{
  appearance: button;
  background-color: #000;
  background-image: none;
  border: 1px solid #000;
  border-radius: 4px;
  box-shadow: #fff 4px 4px 0 0,#000 4px 4px 0 1px;
  box-sizing: border-box;
  color: #fff;
  cursor: pointer;
  display: inline-block;
  font-family: ITCAvantGardeStd-Bk,Arial,sans-serif;
  font-size: 14px;
  font-weight: 400;
  line-height: 20px;
  margin: 0 5px 10px 0;
  overflow: visible;
  padding: 12px 40px;
  text-align: center;
  text-transform: none;
  touch-action: manipulation;
  user-select: none;
  -webkit-user-select: none;
  vertical-align: middle;
  white-space: nowrap;
}}

.button-50:focus {{
  text-decoration: none;
}}

.button-50:hover {{
  text-decoration: none;
}}

.button-50:active {{
  box-shadow: rgba(0, 0, 0, .125) 0 3px 5px inset;
  outline: 0;
}}

.button-50:not([disabled]):active {{
  box-shadow: #fff 2px 2px 0 0, #000 2px 2px 0 1px;
  transform: translate(2px, 2px);
}}

@media (min-width: 768px) {{
  .button-50 {{
    padding: 12px 50px;
  }}
}}
        </style>
        <div>
             <table border='0' cellpadding='0' cellspacing='0' width='555' style='background-color:#e4e4e4;margin-left:auto;margin-right:auto'>
                <tbody>
                    <tr>
                        <td>
                            <table border='0' cellpadding='0' cellspacing='0' width='100%'>
                                <tbody>
                                    <tr>
                                        <td align='center' valign='top' colspan='4'><img tabindex='0'>
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
                                                        <td style='padding-top:15px;padding-bottom:15px;padding-left:30px;font-size:14px;font-family:Tahoma,Arial,Sans-Serif' class='style1'><H2>Redefinição de Senha</H2><tr> 
                                                        <td align='center' valign='top'>__________________________________________________________________</td></tr></td><td>&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td style='padding-left:30px;padding-bottom:20px;font-size:13px;font-family:Tahoma,Arial,Sans-Serif'>Clique aqui para redefinir sua senha:<a style='margin-left:12px;text-decoration:none;' role='button' class='button-50' href='{url}'>REDEFINIR SENHA</a></td>
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
