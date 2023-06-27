using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArteConexao.Migrations
{
    public partial class migration3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Valor",
                table: "ItensReserva",
                newName: "ValorTotal");

            migrationBuilder.RenameColumn(
                name: "Valor",
                table: "ItensCarrinho",
                newName: "ValorTotal");

            migrationBuilder.AddColumn<decimal>(
                name: "ValorReserva",
                table: "ItensReserva",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ValorReserva",
                table: "ItensCarrinho",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValorReserva",
                table: "ItensReserva");

            migrationBuilder.DropColumn(
                name: "ValorReserva",
                table: "ItensCarrinho");

            migrationBuilder.RenameColumn(
                name: "ValorTotal",
                table: "ItensReserva",
                newName: "Valor");

            migrationBuilder.RenameColumn(
                name: "ValorTotal",
                table: "ItensCarrinho",
                newName: "Valor");
        }
    }
}
