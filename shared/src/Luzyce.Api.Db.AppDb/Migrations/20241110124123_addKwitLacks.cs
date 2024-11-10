using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Luzyce.Api.Db.AppDb.Migrations
{
    /// <inheritdoc />
    public partial class addKwitLacks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KwitLacks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LackId = table.Column<int>(type: "int", nullable: false),
                    KwitId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KwitLacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KwitLacks_Documents_KwitId",
                        column: x => x.KwitId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KwitLacks_Errors_LackId",
                        column: x => x.LackId,
                        principalTable: "Errors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Password" },
                values: new object[] { new DateTime(2024, 11, 10, 13, 41, 22, 912, DateTimeKind.Unspecified).AddTicks(1797), "$2a$11$VbhpyFR6tO8vDLkL..j9..xJIhdNY7qjF/Mqsn9k9NiEDlaIsh6ji" });

            migrationBuilder.CreateIndex(
                name: "IX_KwitLacks_KwitId",
                table: "KwitLacks",
                column: "KwitId");

            migrationBuilder.CreateIndex(
                name: "IX_KwitLacks_LackId",
                table: "KwitLacks",
                column: "LackId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KwitLacks");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Password" },
                values: new object[] { new DateTime(2024, 11, 7, 19, 21, 58, 858, DateTimeKind.Unspecified).AddTicks(7925), "$2a$11$suc4Ti3dLipBAZlbJGamxO2JjvQRHiNgdH5tJDcfI7mYCICCL0XRG" });
        }
    }
}
