using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SisAt.Migrations
{
    /// <inheritdoc />
    public partial class quint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CadastroDeHorarioId",
                table: "Agendamentos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CadastroDeHorarioId",
                table: "Agendamentos");
        }
    }
}
