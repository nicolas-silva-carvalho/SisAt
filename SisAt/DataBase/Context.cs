using Microsoft.EntityFrameworkCore;
using SisAt.Models;

namespace SisAt.DataBase
{
    public class Context(DbContextOptions<Context> options) : DbContext(options)
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Agendamento> Agendamentos { get; set; }
        public DbSet<Horarios> Horarios { get; set; }
        public DbSet<CadastroDeHorarios> CadastroDeHorarios { get; set; }
    }
}
