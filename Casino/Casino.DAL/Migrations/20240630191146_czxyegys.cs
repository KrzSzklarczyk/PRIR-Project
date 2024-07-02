using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Casino.DAL.Migrations
{
    /// <inheritdoc />
    public partial class czxyegys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bandits_Games_GameId",
                table: "Bandits");

            migrationBuilder.DropIndex(
                name: "IX_Bandits_GameId",
                table: "Bandits");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "Bandits");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Bandits",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Bandits",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "GameId",
                table: "Bandits",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Bandits_GameId",
                table: "Bandits",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bandits_Games_GameId",
                table: "Bandits",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "GameId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
