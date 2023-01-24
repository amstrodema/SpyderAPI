using Microsoft.EntityFrameworkCore.Migrations;

namespace MainAPI.Data.Migrations
{
    public partial class updateX : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FlagImage",
                table: "Countries",
                newName: "Flag");

            migrationBuilder.AddColumn<string>(
                name: "Abbrv",
                table: "Countries",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Abbrv",
                table: "Countries");

            migrationBuilder.RenameColumn(
                name: "Flag",
                table: "Countries",
                newName: "FlagImage");
        }
    }
}
