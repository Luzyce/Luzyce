using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Luzyce.Api.Db.AppDb.Migrations
{
    /// <inheritdoc />
    public partial class AddVersioningToOrderForProduction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubiektId",
                table: "OrdersForProduction",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "OrdersForProduction",
                type: "int",
                nullable: false,
                defaultValue: 1);
            
            migrationBuilder.Sql("UPDATE OrdersForProduction SET SubiektId = Id, Version = 1 WHERE SubiektId = 0");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Password" },
                values: new object[] { new DateTime(2025, 3, 12, 23, 37, 10, 156, DateTimeKind.Unspecified).AddTicks(568), "$2a$11$3kdc568OWRYiPzAHAtu84uj7hPOuaXziDQcJp/9Aq6mrqOxJp4gbC" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubiektId",
                table: "OrdersForProduction");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "OrdersForProduction");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Password" },
                values: new object[] { new DateTime(2024, 11, 10, 13, 41, 22, 912, DateTimeKind.Unspecified).AddTicks(1797), "$2a$11$VbhpyFR6tO8vDLkL..j9..xJIhdNY7qjF/Mqsn9k9NiEDlaIsh6ji" });
        }
    }
}
