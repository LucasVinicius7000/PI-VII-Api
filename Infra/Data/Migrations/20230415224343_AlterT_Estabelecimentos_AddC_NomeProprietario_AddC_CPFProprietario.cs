using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocalStore.Infra.Data.Migrations
{
    public partial class AlterT_Estabelecimentos_AddC_NomeProprietario_AddC_CPFProprietario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CPFProprietario",
                table: "Estabelecimentos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NomeProprietario",
                table: "Estabelecimentos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CPFProprietario",
                table: "Estabelecimentos");

            migrationBuilder.DropColumn(
                name: "NomeProprietario",
                table: "Estabelecimentos");
        }
    }
}
