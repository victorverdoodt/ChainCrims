using Microsoft.EntityFrameworkCore.Migrations;

namespace ChainCrims.Migrations
{
    public partial class added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "Transacoes",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Transacoes",
                type: "integer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Transacoes");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Transacoes");
        }
    }
}
