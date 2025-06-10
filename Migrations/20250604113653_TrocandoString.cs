using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TotalHealth.Migrations
{
    /// <inheritdoc />
    public partial class TrocandoString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_AspNetUsers_UserId1",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_UserId1",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Usuarios");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Usuarios",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_UserId",
                table: "Usuarios",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_AspNetUsers_UserId",
                table: "Usuarios",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_AspNetUsers_UserId",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_UserId",
                table: "Usuarios");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Usuarios",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Usuarios",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_UserId1",
                table: "Usuarios",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_AspNetUsers_UserId1",
                table: "Usuarios",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
