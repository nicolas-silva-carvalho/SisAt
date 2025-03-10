using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAt.Models;

namespace SisAt.DataBase.Mapping
{
    public class CadastroDeHorariosConfiguration : IEntityTypeConfiguration<CadastroDeHorarios>
    {
        public void Configure(EntityTypeBuilder<CadastroDeHorarios> builder)
        {
            builder.ToTable("CadastroDeHorarios");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.ServicoId)
                .HasColumnName("ServicoId")
                .HasColumnType("INT")
                .IsRequired(true);

            builder.Property(x => x.DataCadastrada)
                .HasColumnName("DataCadastrada")
                .HasColumnType("DATETIME")
                .IsRequired(true);

            builder.Property(x => x.HoraId)
                .HasColumnName("HoraId")
                .HasColumnType("INT")
                .IsRequired(true);

            builder.Property(x => x.Hora)
                .HasColumnName("Hora")
                .HasColumnType("VARCHAR(10)")
                .IsRequired(false);

            builder.Property(x => x.Marcado)
                .HasColumnName("Marcado")
                .HasColumnType("BIT")
                .IsRequired(true);
        }
    }
}
