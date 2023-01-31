using Microsoft.EntityFrameworkCore.Migrations;

namespace MainAPI.Data.Migrations
{
    public partial class cityBoy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Missings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "Missings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Missings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "Marriages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Marriages",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Missings");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "Missings");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Missings");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "Marriages");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Marriages");
        }
    }
}
