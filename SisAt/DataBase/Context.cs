using Microsoft.EntityFrameworkCore;
using SisAt.Models;
using System.Reflection;

namespace SisAt.DataBase
{
    public class Context(DbContextOptions<Context> options) : DbContext(options)
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Agendamento> Agendamentos { get; set; }
        public DbSet<Horarios> Horarios { get; set; }
        public DbSet<CadastroDeHorarios> CadastroDeHorarios { get; set; }
        public DbSet<Calendario> Calendarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
