using Microsoft.EntityFrameworkCore.Migrations;

namespace MainAPI.Data.Migrations
{
    public partial class bankDetailWithdrw : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BankAccountName",
                table: "Withdrawals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankAccountNumber",
                table: "Withdrawals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankName",
                table: "Withdrawals",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankAccountName",
                table: "Withdrawals");

            migrationBuilder.DropColumn(
                name: "BankAccountNumber",
                table: "Withdrawals");

            migrationBuilder.DropColumn(
                name: "BankName",
                table: "Withdrawals");
        }
    }
}
