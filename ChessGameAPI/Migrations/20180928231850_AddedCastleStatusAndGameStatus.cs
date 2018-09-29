using Microsoft.EntityFrameworkCore.Migrations;

namespace ChessGameAPI.Migrations
{
    public partial class AddedCastleStatusAndGameStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Moves_Pieces_PieceId",
                table: "Moves");

            migrationBuilder.DropForeignKey(
                name: "FK_Moves_Users_UserId1",
                table: "Moves");

            migrationBuilder.DropForeignKey(
                name: "FK_Pieces_Users_OwnedById",
                table: "Pieces");

            migrationBuilder.DropIndex(
                name: "IX_Moves_UserId1",
                table: "Moves");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Moves");

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

            migrationBuilder.CreateIndex(
                name: "IX_Moves_UserId",
                table: "Moves",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Moves_Pieces_PieceId",
                table: "Moves",
                column: "PieceId",
                principalTable: "Pieces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Moves_Users_UserId",
                table: "Moves",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pieces_Users_OwnedById",
                table: "Pieces",
                column: "OwnedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Moves_Pieces_PieceId",
                table: "Moves");

            migrationBuilder.DropForeignKey(
                name: "FK_Moves_Users_UserId",
                table: "Moves");

            migrationBuilder.DropForeignKey(
                name: "FK_Pieces_Users_OwnedById",
                table: "Pieces");

            migrationBuilder.DropIndex(
                name: "IX_Moves_UserId",
                table: "Moves");

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

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "Moves",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Moves_UserId1",
                table: "Moves",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Moves_Pieces_PieceId",
                table: "Moves",
                column: "PieceId",
                principalTable: "Pieces",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Moves_Users_UserId1",
                table: "Moves",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Pieces_Users_OwnedById",
                table: "Pieces",
                column: "OwnedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
