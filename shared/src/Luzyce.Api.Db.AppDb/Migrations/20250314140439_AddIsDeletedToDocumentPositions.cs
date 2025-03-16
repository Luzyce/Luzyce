using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Luzyce.Api.Db.AppDb.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDeletedToDocumentPositions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DocumentPositions",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Password" },
                values: new object[] { new DateTime(2025, 3, 14, 15, 4, 38, 575, DateTimeKind.Unspecified).AddTicks(8956), "$2a$11$svwbEi3iahh8PRByR9.cHe4ex/0fOaEOUhm6CeophiWKtnuWZXk.O" });
            
            migrationBuilder.Sql("UPDATE DocumentPositions SET IsDeleted = 1 WHERE DocumentId IN (SELECT Id FROM Documents WHERE StatusId = 6)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DocumentPositions");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Password" },
                values: new object[] { new DateTime(2025, 3, 13, 14, 17, 23, 170, DateTimeKind.Unspecified).AddTicks(54), "$2a$11$Hmijp0b5neGd2oGZS4Hc/uoDq.Q5bhpAGq4EGUwi0NshU6fH.0KoS" });
        }
    }
}
