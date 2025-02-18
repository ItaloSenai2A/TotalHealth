using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TotalHealth.Migrations
{
    /// <inheritdoc />
    public partial class ESP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consultas_Especialidades_EspecialidadeId",
                table: "Consultas");

            migrationBuilder.DropIndex(
                name: "IX_Consultas_EspecialidadeId",
                table: "Consultas");

            migrationBuilder.DropColumn(
                name: "EspecialidadeId",
                table: "Consultas");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EspecialidadeId",
                table: "Consultas",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Consultas_EspecialidadeId",
                table: "Consultas",
                column: "EspecialidadeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Consultas_Especialidades_EspecialidadeId",
                table: "Consultas",
                column: "EspecialidadeId",
                principalTable: "Especialidades",
                principalColumn: "EspecialidadeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
