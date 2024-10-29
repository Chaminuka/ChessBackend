using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChessBackende8.Migrations
{
    /// <inheritdoc />
    public partial class AddCurrPositionColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "currPosition",
                table: "Games",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "currPosition",
                table: "Games");
        }
    }
}
