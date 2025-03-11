using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SisAt.Models;

namespace SisAt.DataBase.Mapping
{
    public class CalendarioConfiguration : IEntityTypeConfiguration<Calendario>
    {
        public void Configure(EntityTypeBuilder<Calendario> builder)
        {
            builder.ToTable("Calendarios");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.title)
                .HasColumnName("title")
                .HasColumnType("VARCHAR(100)")
                .IsRequired(true);

            builder.Property(x => x.start)
                .HasColumnName("start")
                .HasColumnType("DATETIME")
                .IsRequired(true);

            builder.Property(x => x.end)
                .HasColumnName("endDate")
                .HasColumnType("DATETIME")
                .IsRequired(true);

            builder.Property(x => x.borderColor)
                .HasColumnName("borderColor")
                .HasColumnType("VARCHAR(50)")
                .IsRequired(true);

            builder.Property(x => x.backgroundColor)
                .HasColumnName("backgroundColor")
                .HasColumnType("VARCHAR(50)")
                .IsRequired(true);

            builder.Property(x => x.allDay)
                .HasColumnName("allDay")
                .HasColumnType("BIT")
                .IsRequired(true);

            builder.HasOne(x => x.Agendamento)
                .WithOne()
                .HasForeignKey<Calendario>(x => x.AgendamentoId);

            builder.Property(p => p.AgendamentoId)
                .HasColumnName("AgendamentoId")
                .IsRequired(true);
        }
    }
}
