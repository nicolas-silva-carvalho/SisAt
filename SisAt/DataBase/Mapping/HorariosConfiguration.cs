using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAt.Models;

namespace SisAt.DataBase.Mapping
{
    public class HorariosConfiguration : IEntityTypeConfiguration<Horarios>
    {
        public void Configure(EntityTypeBuilder<Horarios> builder)
        {
            builder.ToTable("Horarios");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Hora)
                .HasColumnName("Hora")
                .HasColumnType("VARCHAR(10)")
                .IsRequired(true);

            builder.Property(x => x.Disponivel)
                .HasColumnName("Disponivel")
                .HasColumnType("BIT")
                .IsRequired(false);
        }
    }
}
