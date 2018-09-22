using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ChessGameAPI.Migrations
{
    public partial class AddGameDateTimesMoveDiscriminator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "utcOffset",
                table: "Users",
                nullable: true,
                defaultValue: new DateTimeOffset(new DateTime(1,1,1), new TimeSpan(0,0,0)));

            migrationBuilder.AddColumn<string>(
                name: "PieceDiscriminator",
                table: "Moves",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCompleted",
                table: "Games",
                nullable: true,
                defaultValue: null);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Games",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "utcOffset",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PieceDiscriminator",
                table: "Moves");

            migrationBuilder.DropColumn(
                name: "DateCompleted",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Games");
        }
    }
}
