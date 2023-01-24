using Microsoft.EntityFrameworkCore.Migrations;

namespace MainAPI.Data.Migrations
{
    public partial class T2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GroomName",
                table: "Marriages",
                newName: "GroomLName");

            migrationBuilder.RenameColumn(
                name: "BrideName",
                table: "Marriages",
                newName: "GroomFName");

            migrationBuilder.AddColumn<string>(
                name: "BrideFName",
                table: "Marriages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BrideLName",
                table: "Marriages",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BrideFName",
                table: "Marriages");

            migrationBuilder.DropColumn(
                name: "BrideLName",
                table: "Marriages");

            migrationBuilder.RenameColumn(
                name: "GroomLName",
                table: "Marriages",
                newName: "GroomName");

            migrationBuilder.RenameColumn(
                name: "GroomFName",
                table: "Marriages",
                newName: "BrideName");
        }
    }
}
