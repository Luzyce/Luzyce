using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Luzyce.Api.Db.AppDb.Migrations
{
    /// <inheritdoc />
    public partial class AddSubiektIdToOrderPosition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubiektId",
                table: "OrderPositionsForProduction",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Password" },
                values: new object[] { new DateTime(2025, 3, 13, 14, 17, 23, 170, DateTimeKind.Unspecified).AddTicks(54), "$2a$11$Hmijp0b5neGd2oGZS4Hc/uoDq.Q5bhpAGq4EGUwi0NshU6fH.0KoS" });
            
            migrationBuilder.Sql("UPDATE OrderPositionsForProduction SET SubiektId = Id WHERE SubiektId = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubiektId",
                table: "OrderPositionsForProduction");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Password" },
                values: new object[] { new DateTime(2025, 3, 12, 23, 37, 10, 156, DateTimeKind.Unspecified).AddTicks(568), "$2a$11$3kdc568OWRYiPzAHAtu84uj7hPOuaXziDQcJp/9Aq6mrqOxJp4gbC" });
        }
    }
}
