using Microsoft.EntityFrameworkCore;
using SisAt.DataBase;
using SisAt.Models;
using SisAt.Repository.Persistence.Interfaces;

namespace SisAt.Repository.Persistence
{
    public class UsuarioPersistence : IUsuarioPersistence
    {
        public readonly Context _context;
        public UsuarioPersistence(Context context)
        {
            _context = context;
        }

        public async Task<List<Usuario>> BuscarTodosOsUsuarioAsync()
        {
            return await _context.Usuarios.ToListAsync();
        }

        public async Task<Usuario> BuscarUsuarioPorEmailESenhaAsync(string email, string senha)
        {
            try
            {
                var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower() && x.Senha == senha);
                if (usuario == null) return null;

                return usuario;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Usuario> CriarUsuarioAsync(Usuario usuario)
        {
            try
            {
                usuario.SetaSenhaHash();
                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                return usuario;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
