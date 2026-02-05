using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcompanhamentoPaciente.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPacienteDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataNascimento",
                table: "Pacientes",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Escolaridade",
                table: "Pacientes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Genero",
                table: "Pacientes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Profissao",
                table: "Pacientes",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataNascimento",
                table: "Pacientes");

            migrationBuilder.DropColumn(
                name: "Escolaridade",
                table: "Pacientes");

            migrationBuilder.DropColumn(
                name: "Genero",
                table: "Pacientes");

            migrationBuilder.DropColumn(
                name: "Profissao",
                table: "Pacientes");
        }
    }
}
