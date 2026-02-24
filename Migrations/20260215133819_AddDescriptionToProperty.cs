using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QlChoThueNha1.Migrations
{
    /// <inheritdoc />
    public partial class AddDescriptionToProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Houses_HouseTypes_HouseTypeId",
                table: "Houses");

            migrationBuilder.DropForeignKey(
                name: "FK_RentalRequests_Houses_HouseId",
                table: "RentalRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_RentalRequests_Users_UserId",
                table: "RentalRequests");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "RentalRequests",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "RentalRequests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "RentalRequests",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RequestDate",
                table: "RentalRequests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "RentalRequests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "HouseTypes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Houses",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Houses",
                type: "decimal(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Houses",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Houses",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Houses",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Houses",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "HouseTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Phòng trọ giá rẻ, phù hợp sinh viên", "Phòng trọ" });

            migrationBuilder.UpdateData(
                table: "HouseTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Căn hộ chung cư hiện đại", "Chung cư" });

            migrationBuilder.InsertData(
                table: "HouseTypes",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { 3, "Nhà riêng biệt, không gian rộng rãi", "Nhà nguyên căn" });

            migrationBuilder.UpdateData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Address", "Area", "Description", "ImageUrl", "Name", "Price" },
                values: new object[] { "123 Lê Lợi, Quận 1, TP.HCM", 25.0, "Phòng trọ đẹp, gần chợ Bến Thành, đầy đủ tiện nghi", "/images/house1.jpg", "Phòng trọ Quận 1", 3000000m });

            migrationBuilder.UpdateData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Address", "Description", "ImageUrl", "Name" },
                values: new object[] { "45 Nguyễn Thị Minh Khai, Quận 3, TP.HCM", "Căn hộ 2 phòng ngủ, view đẹp, an ninh tốt", "/images/house2.jpg", "Chung cư Quận 3" });

            migrationBuilder.InsertData(
                table: "Houses",
                columns: new[] { "Id", "Address", "Area", "Description", "HouseTypeId", "ImageUrl", "Name", "Price", "Status" },
                values: new object[,]
                {
                    { 4, "234 Điện Biên Phủ, Quận Bình Thạnh, TP.HCM", 20.0, "Gần trường đại học, tiện cho sinh viên", 1, "/images/house4.jpg", "Phòng trọ Quận Bình Thạnh", 2500000m, "Available" },
                    { 5, "567 Mai Chí Thọ, Quận 2, TP.HCM", 80.0, "Căn hộ cao cấp, đầy đủ nội thất", 2, "/images/house5.jpg", "Chung cư Quận 2", 12000000m, "Rented" }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FullName", "Password" },
                values: new object[] { "Quản trị viên", "123456" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Email", "FullName", "Password", "Username" },
                values: new object[] { "customer1@gmail.com", "Nguyễn Văn A", "123456", "customer1" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FullName", "Password", "Role", "Username" },
                values: new object[] { 3, "customer2@gmail.com", "Trần Thị B", "123456", "Customer", "customer2" });

            migrationBuilder.InsertData(
                table: "Houses",
                columns: new[] { "Id", "Address", "Area", "Description", "HouseTypeId", "ImageUrl", "Name", "Price", "Status" },
                values: new object[] { 3, "89 Huỳnh Tấn Phát, Quận 7, TP.HCM", 120.0, "Nhà 1 trệt 2 lầu, 4 phòng ngủ, có sân vườn", 3, "/images/house3.jpg", "Nhà nguyên căn Quận 7", 15000000m, "Available" });

            migrationBuilder.InsertData(
                table: "RentalRequests",
                columns: new[] { "Id", "EndDate", "HouseId", "Note", "RequestDate", "StartDate", "Status", "UserId" },
                values: new object[] { 1, new DateTime(2024, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, "Thuê dài hạn 1 năm", new DateTime(2023, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Approved", 2 });

            migrationBuilder.AddForeignKey(
                name: "FK_Houses_HouseTypes_HouseTypeId",
                table: "Houses",
                column: "HouseTypeId",
                principalTable: "HouseTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RentalRequests_Houses_HouseId",
                table: "RentalRequests",
                column: "HouseId",
                principalTable: "Houses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RentalRequests_Users_UserId",
                table: "RentalRequests",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Houses_HouseTypes_HouseTypeId",
                table: "Houses");

            migrationBuilder.DropForeignKey(
                name: "FK_RentalRequests_Houses_HouseId",
                table: "RentalRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_RentalRequests_Users_UserId",
                table: "RentalRequests");

            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "RentalRequests",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "HouseTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "RentalRequests");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "RentalRequests");

            migrationBuilder.DropColumn(
                name: "RequestDate",
                table: "RentalRequests");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "RentalRequests");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "HouseTypes");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "RentalRequests",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Houses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Houses",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,0)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Houses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Houses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Houses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Houses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300);

            migrationBuilder.UpdateData(
                table: "HouseTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Chung cư");

            migrationBuilder.UpdateData(
                table: "HouseTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Nhà nguyên căn");

            migrationBuilder.UpdateData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Address", "Area", "Description", "ImageUrl", "Name", "Price" },
                values: new object[] { "123 Lê Lợi", 80.0, "Nhà đẹp trung tâm, gần chợ Bến Thành", "house1.jpg", "Nhà Quận 1", 10000000m });

            migrationBuilder.UpdateData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Address", "Description", "ImageUrl", "Name" },
                values: new object[] { "45 Nguyễn Thị Minh Khai", "Khu dân cư an ninh, thuận tiện đi lại", "house2.jpg", "Nhà Quận 3" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FullName", "Password" },
                values: new object[] { "Administrator", "123" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Email", "FullName", "Password", "Username" },
                values: new object[] { "user@gmail.com", "Khách hàng", "123", "user" });

            migrationBuilder.AddForeignKey(
                name: "FK_Houses_HouseTypes_HouseTypeId",
                table: "Houses",
                column: "HouseTypeId",
                principalTable: "HouseTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RentalRequests_Houses_HouseId",
                table: "RentalRequests",
                column: "HouseId",
                principalTable: "Houses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RentalRequests_Users_UserId",
                table: "RentalRequests",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
