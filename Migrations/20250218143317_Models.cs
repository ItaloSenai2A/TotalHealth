using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TotalHealth.Migrations
{
    /// <inheritdoc />
    public partial class Models : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DataHora",
                table: "Agendamentos",
                newName: "Data");

            migrationBuilder.AlterColumn<decimal>(
                name: "ValorExame",
                table: "Pagamentos",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Agendamentos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Agendamentos");

            migrationBuilder.RenameColumn(
                name: "Data",
                table: "Agendamentos",
                newName: "DataHora");

            migrationBuilder.AlterColumn<decimal>(
                name: "ValorExame",
                table: "Pagamentos",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);
        }
    }
}
