using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArteConexao.Migrations
{
    public partial class migration5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Reservas");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Reservas",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
