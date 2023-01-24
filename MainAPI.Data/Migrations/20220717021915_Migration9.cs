using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MainAPI.Data.Migrations
{
    public partial class Migration9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VoteType",
                table: "Votes");

            migrationBuilder.RenameColumn(
                name: "VoterUserID",
                table: "Votes",
                newName: "UserID");

            migrationBuilder.RenameColumn(
                name: "VoterCountryID",
                table: "Votes",
                newName: "UserCountryID");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Votes",
                newName: "IsLike");

            migrationBuilder.AddColumn<string>(
                name: "BtnBgTypeDisLike",
                table: "Votes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BtnBgTypeLike",
                table: "Votes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserCountryID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsFlagged = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Likes",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserCountryID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BtnBgTypeDisLike = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BtnBgTypeLike = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsLike = table.Column<bool>(type: "bit", nullable: false),
                    IsReact = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Likes", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Likes");

            migrationBuilder.DropColumn(
                name: "BtnBgTypeDisLike",
                table: "Votes");

            migrationBuilder.DropColumn(
                name: "BtnBgTypeLike",
                table: "Votes");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Votes",
                newName: "VoterUserID");

            migrationBuilder.RenameColumn(
                name: "UserCountryID",
                table: "Votes",
                newName: "VoterCountryID");

            migrationBuilder.RenameColumn(
                name: "IsLike",
                table: "Votes",
                newName: "IsActive");

            migrationBuilder.AddColumn<int>(
                name: "VoteType",
                table: "Votes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
