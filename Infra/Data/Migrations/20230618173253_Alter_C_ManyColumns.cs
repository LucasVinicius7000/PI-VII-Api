using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocalStore.Infra.Data.Migrations
{
    public partial class Alter_C_ManyColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QuantidadeEstoque",
                table: "ProdutoPedido",
                newName: "VendidoPor");

            migrationBuilder.AlterColumn<double>(
                name: "QuantidadeEstoque",
                table: "Produtos",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<DateTime>(
                name: "VencimentoEm",
                table: "ProdutoPedido",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<double>(
                name: "ValorComDesconto",
                table: "ProdutoPedido",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "Peso",
                table: "ProdutoPedido",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<int>(
                name: "ProdutoOriginalId",
                table: "ProdutoPedido",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "QuantidadePedido",
                table: "ProdutoPedido",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UnidadeMedida",
                table: "ProdutoPedido",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "FormaPagamento",
                table: "Pedidos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProdutoOriginalId",
                table: "ProdutoPedido");

            migrationBuilder.DropColumn(
                name: "QuantidadePedido",
                table: "ProdutoPedido");

            migrationBuilder.DropColumn(
                name: "UnidadeMedida",
                table: "ProdutoPedido");

            migrationBuilder.RenameColumn(
                name: "VendidoPor",
                table: "ProdutoPedido",
                newName: "QuantidadeEstoque");

            migrationBuilder.AlterColumn<double>(
                name: "QuantidadeEstoque",
                table: "Produtos",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "VencimentoEm",
                table: "ProdutoPedido",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ValorComDesconto",
                table: "ProdutoPedido",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Peso",
                table: "ProdutoPedido",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FormaPagamento",
                table: "Pedidos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
