using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShippingApp.Migrations
{
    /// <inheritdoc />
    public partial class third : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "userId",
                keyValue: new Guid("f2063f71-e145-4c3c-bc5a-70b4d23f9566"));

            migrationBuilder.CreateTable(
                name: "TransactionRecords",
                columns: table => new
                {
                    transactionRecordsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    orderId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    paymentId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    transactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    dateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    isPaid = table.Column<bool>(type: "bit", nullable: false),
                    amount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionRecords", x => x.transactionRecordsId);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "userId", "address", "contactNo", "createdAt", "email", "firstName", "isDeleted", "lastName", "otpUsableTill", "passwordHash", "pathToProfilePic", "token", "updatedAt", "userRole", "verificationOTP", "verifiedAt" },
                values: new object[] { new Guid("59e4843d-f2a5-4570-acf7-f2d814e1738c"), "address", 9865326598L, new DateTime(2023, 5, 9, 12, 29, 41, 803, DateTimeKind.Local).AddTicks(3205), "admin@gmail.com", "Admin", false, "Admin", new DateTime(2023, 5, 9, 12, 29, 41, 803, DateTimeKind.Local).AddTicks(3216), new byte[] { 8, 151, 35, 64, 98, 188, 240, 221, 74, 208, 219, 16, 70, 134, 10, 16, 57, 83, 242, 218, 134, 209, 18, 168, 96, 254, 75, 50, 249, 152, 36, 152, 154, 235, 192, 53, 118, 215, 167, 35, 151, 189, 180, 117, 133, 22, 166, 2, 234, 12, 105, 26, 205, 50, 126, 134, 244, 220, 11, 18, 168, 167, 141, 144 }, "pathToProfilePic", "", new DateTime(2023, 5, 9, 12, 29, 41, 803, DateTimeKind.Local).AddTicks(3215), "admin", 999, new DateTime(2023, 5, 9, 12, 29, 41, 803, DateTimeKind.Local).AddTicks(3217) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransactionRecords");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "userId",
                keyValue: new Guid("59e4843d-f2a5-4570-acf7-f2d814e1738c"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "userId", "address", "contactNo", "createdAt", "email", "firstName", "isDeleted", "lastName", "otpUsableTill", "passwordHash", "pathToProfilePic", "token", "updatedAt", "userRole", "verificationOTP", "verifiedAt" },
                values: new object[] { new Guid("f2063f71-e145-4c3c-bc5a-70b4d23f9566"), "address", 9865326598L, new DateTime(2023, 4, 11, 13, 1, 53, 359, DateTimeKind.Local).AddTicks(5771), "admin@gmail.com", "Admin", false, "Admin", new DateTime(2023, 4, 11, 13, 1, 53, 359, DateTimeKind.Local).AddTicks(5781), new byte[] { 8, 151, 35, 64, 98, 188, 240, 221, 74, 208, 219, 16, 70, 134, 10, 16, 57, 83, 242, 218, 134, 209, 18, 168, 96, 254, 75, 50, 249, 152, 36, 152, 154, 235, 192, 53, 118, 215, 167, 35, 151, 189, 180, 117, 133, 22, 166, 2, 234, 12, 105, 26, 205, 50, 126, 134, 244, 220, 11, 18, 168, 167, 141, 144 }, "pathToProfilePic", "", new DateTime(2023, 4, 11, 13, 1, 53, 359, DateTimeKind.Local).AddTicks(5780), "admin", 999, new DateTime(2023, 4, 11, 13, 1, 53, 359, DateTimeKind.Local).AddTicks(5782) });
        }
    }
}
