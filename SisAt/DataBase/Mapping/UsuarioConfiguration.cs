using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAt.Models;

namespace SisAt.DataBase.Mapping
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuarios");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Nome)
                .HasColumnName("Nome")
                .HasColumnType("VARCHAR(50)")
                .IsRequired(true);

            builder.Property(x => x.Email)
                .HasColumnName("Email")
                .HasColumnType("VARCHAR(50)")
                .IsRequired(true);

            builder.Property(x => x.Senha)
                .HasColumnName("Senha")
                .HasColumnType("VARCHAR(50)")
                .IsRequired(true);

            builder.Property(x => x.Ativo)
                .HasColumnName("Ativo")
                .HasColumnType("bit")
                .IsRequired(true);
        }
    }
}
