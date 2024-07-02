using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Casino.DAL.Migrations
{
    /// <inheritdoc />
    public partial class bhicxhbuzdsbfysdbfgy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Roulettes_Games_GameId",
                table: "Roulettes");

            migrationBuilder.DropIndex(
                name: "IX_Roulettes_GameId",
                table: "Roulettes");

            migrationBuilder.RenameColumn(
                name: "BetType",
                table: "Roulettes",
                newName: "number");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Roulettes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<bool>(
                name: "Black",
                table: "Roulettes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Red",
                table: "Roulettes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Black",
                table: "Roulettes");

            migrationBuilder.DropColumn(
                name: "Red",
                table: "Roulettes");

            migrationBuilder.RenameColumn(
                name: "number",
                table: "Roulettes",
                newName: "BetType");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Roulettes",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roulettes_GameId",
                table: "Roulettes",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Roulettes_Games_GameId",
                table: "Roulettes",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "GameId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
