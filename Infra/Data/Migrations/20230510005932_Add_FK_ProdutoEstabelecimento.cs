using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocalStore.Infra.Data.Migrations
{
    public partial class Add_FK_ProdutoEstabelecimento : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Produtos_EstabelecimentoId",
                table: "Produtos",
                column: "EstabelecimentoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Produtos_Estabelecimentos_EstabelecimentoId",
                table: "Produtos",
                column: "EstabelecimentoId",
                principalTable: "Estabelecimentos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Produtos_Estabelecimentos_EstabelecimentoId",
                table: "Produtos");

            migrationBuilder.DropIndex(
                name: "IX_Produtos_EstabelecimentoId",
                table: "Produtos");
        }
    }
}
