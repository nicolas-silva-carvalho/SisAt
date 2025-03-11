using SisAt.Models;

namespace SisAt.Sessao
{
    public interface ISessaoFactory
    {
        void CriarSessao(Usuario usuario);
        Usuario RecuperarSessaoId();
        void RemoverSessaoPorId();
    }
}