using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShippingApp.Migrations
{
    /// <inheritdoc />
    public partial class second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "contactno",
                table: "Users",
                newName: "contactNo");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "userId", "address", "contactNo", "createdAt", "email", "firstName", "isDeleted", "lastName", "otpUsableTill", "passwordHash", "pathToProfilePic", "token", "updatedAt", "userRole", "verificationOTP", "verifiedAt" },
                values: new object[] { new Guid("f2063f71-e145-4c3c-bc5a-70b4d23f9566"), "address", 9865326598L, new DateTime(2023, 4, 11, 13, 1, 53, 359, DateTimeKind.Local).AddTicks(5771), "admin@gmail.com", "Admin", false, "Admin", new DateTime(2023, 4, 11, 13, 1, 53, 359, DateTimeKind.Local).AddTicks(5781), new byte[] { 8, 151, 35, 64, 98, 188, 240, 221, 74, 208, 219, 16, 70, 134, 10, 16, 57, 83, 242, 218, 134, 209, 18, 168, 96, 254, 75, 50, 249, 152, 36, 152, 154, 235, 192, 53, 118, 215, 167, 35, 151, 189, 180, 117, 133, 22, 166, 2, 234, 12, 105, 26, 205, 50, 126, 134, 244, 220, 11, 18, 168, 167, 141, 144 }, "pathToProfilePic", "", new DateTime(2023, 4, 11, 13, 1, 53, 359, DateTimeKind.Local).AddTicks(5780), "admin", 999, new DateTime(2023, 4, 11, 13, 1, 53, 359, DateTimeKind.Local).AddTicks(5782) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "userId",
                keyValue: new Guid("f2063f71-e145-4c3c-bc5a-70b4d23f9566"));

            migrationBuilder.RenameColumn(
                name: "contactNo",
                table: "Users",
                newName: "contactno");
        }
    }
}
