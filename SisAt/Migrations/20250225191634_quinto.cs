using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SisAt.Migrations
{
    /// <inheritdoc />
    public partial class quinto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServicoId",
                table: "Agendamentos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ServicoId",
                table: "Agendamentos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
