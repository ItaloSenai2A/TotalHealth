using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TotalHealth.Migrations
{
    /// <inheritdoc />
    public partial class usuarios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UsuariosLogin",
                columns: table => new
                {
                    UsuarioLoginId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Cargo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId1 = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuariosLogin", x => x.UsuarioLoginId);
                    table.ForeignKey(
                        name: "FK_UsuariosLogin_AspNetUsers_UserId1",
                        column: x => x.UserId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosLogin_UserId1",
                table: "UsuariosLogin",
                column: "UserId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsuariosLogin");
        }
    }
}
