using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAt.Models;

namespace SisAt.DataBase.Mapping
{
    public class AgendamentosConfiguration : IEntityTypeConfiguration<Agendamento>
    {
        public void Configure(EntityTypeBuilder<Agendamento> builder)
        {
            builder.ToTable("Agendamentos");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.CpfCnpj)
                .HasColumnName("CpfCnpj")
                .HasColumnType("VARCHAR(50)")
                .IsRequired(true);

            builder.HasOne(x => x.CadastroDeHorarios).WithOne().HasForeignKey<Agendamento>(x => x.CadastroDeHorarioId);

            builder.Property(x => x.CadastroDeHorarioId)
                .HasColumnName("CadastroDeHorarioId")
                .IsRequired(true);

            builder.Property(x => x.Nome)
                .HasColumnName("Nome")
                .HasColumnType("VARCHAR(50)")
                .IsRequired(true);

            builder.Property(x => x.Email)
                .HasColumnName("Email")
                .HasColumnType("VARCHAR(50)")
                .IsRequired(false);

            builder.Property(x => x.ServicoId)
                .HasColumnName("ServicoId")
                .HasColumnType("INT")
                .IsRequired(true);

            builder.Property(x => x.ServicoNome)
                .HasColumnName("ServicoNome")
                .HasColumnType("VARCHAR(50)")
                .IsRequired(true);

            builder.Property(x => x.DataMarcada)
                .HasColumnName("DataMarcada")
                .HasColumnType("DATETIME")
                .IsRequired(true);

            builder.Property(x => x.Hora)
                .HasColumnName("Hora")
                .HasColumnType("VARCHAR(10)")
                .IsRequired(true);

            builder.Property(x => x.Protocolo)
                .HasColumnName("Protocolo")
                .HasColumnType("VARCHAR(50)")
                .IsRequired(false);

            builder.Property(x => x.ConfirmarAgendamento)
                .HasColumnName("ConfirmarAgendamento")
                .HasColumnType("BIT")
                .IsRequired(false);
        }
    }
}
