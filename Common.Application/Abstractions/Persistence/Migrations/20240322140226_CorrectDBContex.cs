using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Common.Application.Migrations
{
    /// <inheritdoc />
    public partial class CorrectDBContex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ApplicationUserRoles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "User" }
                });

            migrationBuilder.InsertData(
                table: "ApplicationUsers",
                columns: new[] { "Id", "Login", "PasswordHash", "UserName" },
                values: new object[] { 1, "admin", "Yx7jh/W92kRLgdCk9026HJsBIp13DmXV6XZ5lpXJF3RQs0DxA/r31TtLQVkcVy8E4EfIP15zRcocL6YAXt3h5Q==BB3CA5B46C5E510AF1A7ADC089A3211F85926564A0D6B709FADF5C44A0BA8C3988EE3AC041C8B4E7071037C3BB2A59B16A6A3A41E9E2A4904BE52A48A02C8884", "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ApplicationUserRoles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ApplicationUserRoles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ApplicationUsers",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
