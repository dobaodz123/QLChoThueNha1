using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QlChoThueNha1.Migrations
{
    /// <inheritdoc />
    public partial class CreateThanhToan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$CNUsUpn97pqWvh3YeQcXLOgS.DD/8/hldun6O5xsmPQrE4jBoKvr2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$2m1uEbztcnx7hUhC3PrR8ejYVRAtsXlGAwvyi9GVoFlGy/vHoCiO.");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3,
                column: "Password",
                value: "$2a$11$yO/KWg7hLkefX5bUS4U4Ze7Bc65C2On.tH//gmffoNp6g8HH1d6Xu");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "Password",
                value: "123456");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                column: "Password",
                value: "123456");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3,
                column: "Password",
                value: "123456");
        }
    }
}
