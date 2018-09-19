using Microsoft.EntityFrameworkCore.Migrations;

namespace ChessGameAPI.Migrations
{
    public partial class AddedForeignKeyFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "WhiteUserId",
                table: "Games",
                nullable: true);
            migrationBuilder.AlterColumn<string>(
                name: "BlackUserId",
                table: "Games",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "WhiteUserId",
                table: "Games",
                nullable: false);
            migrationBuilder.AlterColumn<string>(
                name: "BlackUserId",
                table: "Games",
                nullable: false);
        }
    }
}
