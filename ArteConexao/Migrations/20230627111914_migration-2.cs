using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArteConexao.Migrations
{
    public partial class migration2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagemUrl",
                table: "ItensReserva",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImagemUrl",
                table: "ItensCarrinho",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "StandId",
                table: "ItensCarrinho",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagemUrl",
                table: "ItensReserva");

            migrationBuilder.DropColumn(
                name: "ImagemUrl",
                table: "ItensCarrinho");

            migrationBuilder.DropColumn(
                name: "StandId",
                table: "ItensCarrinho");
        }
    }
}
