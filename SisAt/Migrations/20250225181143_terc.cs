using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SisAt.Migrations
{
    /// <inheritdoc />
    public partial class terc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Endereco",
                table: "Agendamentos");

            migrationBuilder.DropColumn(
                name: "RG",
                table: "Agendamentos");

            migrationBuilder.AddColumn<bool>(
                name: "Marcado",
                table: "CadastroDeHorarios",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Hora",
                table: "Agendamentos",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Marcado",
                table: "CadastroDeHorarios");

            migrationBuilder.DropColumn(
                name: "Hora",
                table: "Agendamentos");

            migrationBuilder.AddColumn<string>(
                name: "Endereco",
                table: "Agendamentos",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RG",
                table: "Agendamentos",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
