using SisAt.Models;

namespace SisAt.Repository.Persistence.Interfaces
{
    public interface IUsuarioPersistence
    {
        Task<Usuario> BuscarUsuarioPorEmailESenhaAsync(string email, string senha);
        Task<Usuario> CriarUsuarioAsync(Usuario usuario);
        Task<List<Usuario>> BuscarTodosOsUsuarioAsync();
    }
}
