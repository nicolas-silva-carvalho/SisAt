using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SisAt.Migrations
{
    /// <inheritdoc />
    public partial class quat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Local",
                table: "Agendamentos");

            migrationBuilder.DropColumn(
                name: "servico",
                table: "Agendamentos");

            migrationBuilder.AlterColumn<string>(
                name: "Hora",
                table: "Agendamentos",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataMarcada",
                table: "Agendamentos",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ServicoId",
                table: "Agendamentos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServicoId",
                table: "Agendamentos");

            migrationBuilder.AlterColumn<string>(
                name: "Hora",
                table: "Agendamentos",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataMarcada",
                table: "Agendamentos",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AddColumn<string>(
                name: "Local",
                table: "Agendamentos",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "servico",
                table: "Agendamentos",
                type: "TEXT",
                nullable: true);
        }
    }
}
