using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MainAPI.Data.Migrations
{
    public partial class Migration6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreatedBy",
                table: "Petitions");

            migrationBuilder.DropColumn(
                name: "DateModifiedBy",
                table: "Petitions");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Petitions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "Petitions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Petitions");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "Petitions");

            migrationBuilder.AddColumn<string>(
                name: "DateCreatedBy",
                table: "Petitions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DateModifiedBy",
                table: "Petitions",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
