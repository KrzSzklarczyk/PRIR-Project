using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Casino.DAL.Migrations
{
    /// <inheritdoc />
    public partial class bhicxhbuzdsbfysdbfgy1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Roulettes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "betnumber",
                table: "Roulettes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "betnumber",
                table: "Roulettes");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Roulettes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
