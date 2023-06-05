using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocalStore.Infra.Data.Migrations
{
    public partial class Alter_C_Estabelecimento : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FormasPagamentoAceitas",
                table: "Estabelecimentos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MetodoCompra",
                table: "Estabelecimentos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "TaxaKmRodado",
                table: "Estabelecimentos",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TaxaMinimaEntrega",
                table: "Estabelecimentos",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "UrlAlvaraFuncionamento",
                table: "Estabelecimentos",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FormasPagamentoAceitas",
                table: "Estabelecimentos");

            migrationBuilder.DropColumn(
                name: "MetodoCompra",
                table: "Estabelecimentos");

            migrationBuilder.DropColumn(
                name: "TaxaKmRodado",
                table: "Estabelecimentos");

            migrationBuilder.DropColumn(
                name: "TaxaMinimaEntrega",
                table: "Estabelecimentos");

            migrationBuilder.DropColumn(
                name: "UrlAlvaraFuncionamento",
                table: "Estabelecimentos");
        }
    }
}
