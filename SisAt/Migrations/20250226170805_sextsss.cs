using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SisAt.Migrations
{
    /// <inheritdoc />
    public partial class sextsss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CadastroDeHorariosId",
                table: "Agendamentos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Agendamentos_CadastroDeHorariosId",
                table: "Agendamentos",
                column: "CadastroDeHorariosId");

            migrationBuilder.AddForeignKey(
                name: "FK_Agendamentos_CadastroDeHorarios_CadastroDeHorariosId",
                table: "Agendamentos",
                column: "CadastroDeHorariosId",
                principalTable: "CadastroDeHorarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agendamentos_CadastroDeHorarios_CadastroDeHorariosId",
                table: "Agendamentos");

            migrationBuilder.DropIndex(
                name: "IX_Agendamentos_CadastroDeHorariosId",
                table: "Agendamentos");

            migrationBuilder.DropColumn(
                name: "CadastroDeHorariosId",
                table: "Agendamentos");
        }
    }
}
