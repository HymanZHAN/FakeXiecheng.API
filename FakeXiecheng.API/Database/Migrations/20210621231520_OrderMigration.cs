using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FakeXiecheng.API.Database.Migrations
{
    public partial class OrderMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                table: "LineItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    State = table.Column<int>(type: "int", nullable: false),
                    CreateDateUTC = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TransactionMetaData = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "547c1a4c-b463-43e3-8e08-03bf8b3bfb23",
                column: "ConcurrencyStamp",
                value: "d0924493-547c-4ef9-a57e-1230cd00398a");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "959bc2f6-9226-4e91-a288-15e62ff90a58",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "917f9a57-1194-4790-a2d9-9d6fb2a2e2b4", "AQAAAAEAACcQAAAAELJJSuZSl/26HcAn3NoDeay4fNENmit7kcxedaGK0/pq2QPfNyd4VuSyjZB5CL9wHw==", "c59194a8-a1f5-4e17-b88b-03d1adefd4e8" });

            migrationBuilder.CreateIndex(
                name: "IX_LineItems_OrderId",
                table: "LineItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_LineItems_Orders_OrderId",
                table: "LineItems",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LineItems_Orders_OrderId",
                table: "LineItems");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_LineItems_OrderId",
                table: "LineItems");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "LineItems");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "547c1a4c-b463-43e3-8e08-03bf8b3bfb23",
                column: "ConcurrencyStamp",
                value: "3ca91efa-4d22-4037-8b74-57b6beea4878");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "959bc2f6-9226-4e91-a288-15e62ff90a58",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2f83ed9f-4d5f-4d4e-a85e-b93fbc765a65", "AQAAAAEAACcQAAAAEKr/485MYc+equlLrzlwzvjtsNOAS/cAZT82wMNp4qOY+npZMTmXhm15Utrf8nVcvg==", "7399cb10-eb4e-4009-9189-a85d062bae90" });
        }
    }
}
