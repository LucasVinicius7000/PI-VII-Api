using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocalStore.Infra.Data.Migrations
{
    public partial class Alter_T_Produtos_Alter_C_ToVendidoPor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VendidoPorPeso",
                table: "Produtos");

            migrationBuilder.AddColumn<int>(
                name: "VendidoPor",
                table: "Produtos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VendidoPor",
                table: "Produtos");

            migrationBuilder.AddColumn<bool>(
                name: "VendidoPorPeso",
                table: "Produtos",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
