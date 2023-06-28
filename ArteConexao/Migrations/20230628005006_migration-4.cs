using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArteConexao.Migrations
{
    public partial class migration4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagemUrl",
                table: "ItensReserva");

            migrationBuilder.DropColumn(
                name: "ValorTotal",
                table: "ItensReserva");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ItensReserva",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "ItensReserva");

            migrationBuilder.AddColumn<string>(
                name: "ImagemUrl",
                table: "ItensReserva",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "ValorTotal",
                table: "ItensReserva",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
