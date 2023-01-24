using Microsoft.EntityFrameworkCore.Migrations;

namespace MainAPI.Data.Migrations
{
    public partial class DFlex3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsGlobalRange",
                table: "Settings",
                newName: "IsShowPhoneNo");

            migrationBuilder.AddColumn<bool>(
                name: "IsAllowAccess",
                table: "Settings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAnoymousMessaging",
                table: "Settings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLocalRange",
                table: "Settings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsReactionNotification",
                table: "Settings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRecieveAnoymousMessages",
                table: "Settings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSendNotificationToMail",
                table: "Settings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAllowAccess",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "IsAnoymousMessaging",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "IsLocalRange",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "IsReactionNotification",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "IsRecieveAnoymousMessages",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "IsSendNotificationToMail",
                table: "Settings");

            migrationBuilder.RenameColumn(
                name: "IsShowPhoneNo",
                table: "Settings",
                newName: "IsGlobalRange");
        }
    }
}
