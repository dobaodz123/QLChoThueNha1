using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QlChoThueNha1.Migrations
{
    /// <inheritdoc />
    public partial class AddTrangThaiThanhToan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TrangThai",
                table: "ThanhToans",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrangThai",
                table: "ThanhToans");
        }
    }
}
