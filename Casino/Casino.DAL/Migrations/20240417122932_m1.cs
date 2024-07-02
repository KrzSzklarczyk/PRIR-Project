using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Casino.DAL.Migrations
{
    /// <inheritdoc />
    public partial class m1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    NickName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Credits = table.Column<int>(type: "int", nullable: false),
                    UserType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    TransactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_Transactions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bandits",
                columns: table => new
                {
                    BanditId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Position1 = table.Column<int>(type: "int", nullable: false),
                    Position2 = table.Column<int>(type: "int", nullable: false),
                    Position3 = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bandits", x => x.BanditId);
                });

            migrationBuilder.CreateTable(
                name: "BlackJacks",
                columns: table => new
                {
                    BlackJackId = table.Column<int>(type: "int", nullable: false),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DealerHunt = table.Column<int>(type: "int", nullable: false),
                    UserHunt = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlackJacks", x => x.BlackJackId);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    GameId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResultId = table.Column<int>(type: "int", nullable: true),
                    BlackJackId = table.Column<int>(type: "int", nullable: true),
                    RouletteId = table.Column<int>(type: "int", nullable: true),
                    BanditId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaxBet = table.Column<int>(type: "int", nullable: false),
                    MinBet = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.GameId);
                    table.ForeignKey(
                        name: "FK_Games_Bandits_BanditId",
                        column: x => x.BanditId,
                        principalTable: "Bandits",
                        principalColumn: "BanditId");
                    table.ForeignKey(
                        name: "FK_Games_BlackJacks_BlackJackId",
                        column: x => x.BlackJackId,
                        principalTable: "BlackJacks",
                        principalColumn: "BlackJackId");
                });

            migrationBuilder.CreateTable(
                name: "Results",
                columns: table => new
                {
                    ResultId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Results", x => x.ResultId);
                    table.ForeignKey(
                        name: "FK_Results_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "GameId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Results_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Roulettes",
                columns: table => new
                {
                    RouletteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    BetType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roulettes", x => x.RouletteId);
                    table.ForeignKey(
                        name: "FK_Roulettes_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "GameId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bandits_GameId",
                table: "Bandits",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_BanditId",
                table: "Games",
                column: "BanditId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_BlackJackId",
                table: "Games",
                column: "BlackJackId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_ResultId",
                table: "Games",
                column: "ResultId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_RouletteId",
                table: "Games",
                column: "RouletteId");

            migrationBuilder.CreateIndex(
                name: "IX_Results_GameId",
                table: "Results",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Results_UserId",
                table: "Results",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Roulettes_GameId",
                table: "Roulettes",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_UserId",
                table: "Transactions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bandits_Games_GameId",
                table: "Bandits",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "GameId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BlackJacks_Games_BlackJackId",
                table: "BlackJacks",
                column: "BlackJackId",
                principalTable: "Games",
                principalColumn: "GameId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Results_ResultId",
                table: "Games",
                column: "ResultId",
                principalTable: "Results",
                principalColumn: "ResultId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Roulettes_RouletteId",
                table: "Games",
                column: "RouletteId",
                principalTable: "Roulettes",
                principalColumn: "RouletteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bandits_Games_GameId",
                table: "Bandits");

            migrationBuilder.DropForeignKey(
                name: "FK_BlackJacks_Games_BlackJackId",
                table: "BlackJacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Results_Games_GameId",
                table: "Results");

            migrationBuilder.DropForeignKey(
                name: "FK_Roulettes_Games_GameId",
                table: "Roulettes");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Bandits");

            migrationBuilder.DropTable(
                name: "BlackJacks");

            migrationBuilder.DropTable(
                name: "Results");

            migrationBuilder.DropTable(
                name: "Roulettes");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
