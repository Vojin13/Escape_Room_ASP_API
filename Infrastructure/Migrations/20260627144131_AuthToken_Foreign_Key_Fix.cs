using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AuthToken_Foreign_Key_Fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthTokens_Users_UserId1",
                table: "AuthTokens");

            migrationBuilder.DropIndex(
                name: "IX_AuthTokens_UserId1",
                table: "AuthTokens");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "AuthTokens");

            migrationBuilder.UpdateData(
                table: "BookingStatuses",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 41, 30, 628, DateTimeKind.Utc).AddTicks(9952));

            migrationBuilder.UpdateData(
                table: "BookingStatuses",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 41, 30, 628, DateTimeKind.Utc).AddTicks(9955));

            migrationBuilder.UpdateData(
                table: "BookingStatuses",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 41, 30, 628, DateTimeKind.Utc).AddTicks(9956));

            migrationBuilder.UpdateData(
                table: "BookingStatuses",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 41, 30, 628, DateTimeKind.Utc).AddTicks(9957));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "AuthTokens",
                type: "integer",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "BookingStatuses",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 37, 9, 481, DateTimeKind.Utc).AddTicks(2156));

            migrationBuilder.UpdateData(
                table: "BookingStatuses",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 37, 9, 481, DateTimeKind.Utc).AddTicks(2160));

            migrationBuilder.UpdateData(
                table: "BookingStatuses",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 37, 9, 481, DateTimeKind.Utc).AddTicks(2161));

            migrationBuilder.UpdateData(
                table: "BookingStatuses",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 37, 9, 481, DateTimeKind.Utc).AddTicks(2162));

            migrationBuilder.CreateIndex(
                name: "IX_AuthTokens_UserId1",
                table: "AuthTokens",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthTokens_Users_UserId1",
                table: "AuthTokens",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
