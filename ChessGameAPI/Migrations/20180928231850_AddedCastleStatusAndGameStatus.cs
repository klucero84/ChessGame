using Microsoft.EntityFrameworkCore.Migrations;

namespace ChessGameAPI.Migrations
{
    public partial class AddedCastleStatusAndGameStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AlgebraicNotation",
                table: "Moves",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CanBlackKingSideCastle",
                table: "Games",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanBlackQueenSideCastle",
                table: "Games",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanWhiteKingSideCastle",
                table: "Games",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanWhiteQueenSideCastle",
                table: "Games",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "statusCode",
                table: "Games",
                nullable: false,
                defaultValue: 0);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.DropColumn(
                name: "AlgebraicNotation",
                table: "Moves");

            migrationBuilder.DropColumn(
                name: "CanBlackKingSideCastle",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "CanBlackQueenSideCastle",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "CanWhiteKingSideCastle",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "CanWhiteQueenSideCastle",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "statusCode",
                table: "Games");
        }
    }
}
