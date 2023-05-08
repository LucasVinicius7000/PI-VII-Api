using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocalStore.Infra.Data.Migrations
{
    public partial class Add_C_Email_T_Estabelecimentos2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Estabelecimentos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Estabelecimentos");
        }
    }
}
