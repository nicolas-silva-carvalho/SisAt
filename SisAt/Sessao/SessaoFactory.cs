using Newtonsoft.Json;
using SisAt.Models;

namespace SisAt.Sessao
{
    public class SessaoFactory : ISessaoFactory
    {
        private readonly IHttpContextAccessor httpContext;

        public SessaoFactory(IHttpContextAccessor httpContext)
        {
            this.httpContext = httpContext;
        }

        public void CriarSessao(Usuario usuario)
        {
            string sessaoUsuario = JsonConvert.SerializeObject(usuario);

            httpContext.HttpContext.Session.SetString("SessaoLogado", sessaoUsuario);
            string horaInicioSessao = DateTime.Now.ToString("o");
            httpContext.HttpContext.Session.SetString("SessaoInicio", horaInicioSessao);
        }

        public Usuario RecuperarSessaoId()
        {
            string sessaoUsuario = httpContext.HttpContext.Session.GetString("SessaoLogado");

            if (string.IsNullOrEmpty(sessaoUsuario)) return null;

            return JsonConvert.DeserializeObject<Usuario>(sessaoUsuario);
        }

        public TimeSpan RecuperarTempoSessao()
        {
            string sessaoInicio = httpContext.HttpContext.Session.GetString("SessaoInicio");
            if (DateTime.TryParse(sessaoInicio, out DateTime dataInicioSessao))
            {
                return DateTime.Now - dataInicioSessao;
            }
            return TimeSpan.Zero;
        }

        public void RemoverSessaoPorId()
        {
            httpContext.HttpContext.Session.Remove("SessaoLogado");
            httpContext.HttpContext.Session.Remove("SessaoInicio");
        }
    }
}
