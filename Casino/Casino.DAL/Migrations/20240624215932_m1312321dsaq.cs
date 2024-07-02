using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Casino.DAL.Migrations
{
    /// <inheritdoc />
    public partial class m1312321dsaq : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_BlackJacks_BlackJackId",
                table: "Games");

            migrationBuilder.DropTable(
                name: "BlackJacks");

            migrationBuilder.DropIndex(
                name: "IX_Games_BlackJackId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "BlackJackId",
                table: "Games");

            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "BlackJackId",
                table: "Games",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BlackJacks",
                columns: table => new
                {
                    BlackJackId = table.Column<int>(type: "int", nullable: false),
                    DealerHunt = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    UserHunt = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlackJacks", x => x.BlackJackId);
                    table.ForeignKey(
                        name: "FK_BlackJacks_Games_BlackJackId",
                        column: x => x.BlackJackId,
                        principalTable: "Games",
                        principalColumn: "GameId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Games_BlackJackId",
                table: "Games",
                column: "BlackJackId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_BlackJacks_BlackJackId",
                table: "Games",
                column: "BlackJackId",
                principalTable: "BlackJacks",
                principalColumn: "BlackJackId");
        }
    }
}
