using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UserSoftDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "BookingStatuses",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 23, 59, 31, 839, DateTimeKind.Utc).AddTicks(319));

            migrationBuilder.UpdateData(
                table: "BookingStatuses",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 23, 59, 31, 839, DateTimeKind.Utc).AddTicks(325));

            migrationBuilder.UpdateData(
                table: "BookingStatuses",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 23, 59, 31, 839, DateTimeKind.Utc).AddTicks(326));

            migrationBuilder.UpdateData(
                table: "BookingStatuses",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 23, 59, 31, 839, DateTimeKind.Utc).AddTicks(327));

            migrationBuilder.UpdateData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 23, 59, 31, 839, DateTimeKind.Utc).AddTicks(2081));

            migrationBuilder.UpdateData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 23, 59, 31, 839, DateTimeKind.Utc).AddTicks(2082));

            migrationBuilder.UpdateData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 23, 59, 31, 839, DateTimeKind.Utc).AddTicks(2083));

            migrationBuilder.UpdateData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 23, 59, 31, 839, DateTimeKind.Utc).AddTicks(2084));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "BookingStatuses",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 42, 23, 669, DateTimeKind.Utc).AddTicks(3720));

            migrationBuilder.UpdateData(
                table: "BookingStatuses",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 42, 23, 669, DateTimeKind.Utc).AddTicks(3723));

            migrationBuilder.UpdateData(
                table: "BookingStatuses",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 42, 23, 669, DateTimeKind.Utc).AddTicks(3724));

            migrationBuilder.UpdateData(
                table: "BookingStatuses",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 42, 23, 669, DateTimeKind.Utc).AddTicks(3725));

            migrationBuilder.UpdateData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 42, 23, 669, DateTimeKind.Utc).AddTicks(5635));

            migrationBuilder.UpdateData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 42, 23, 669, DateTimeKind.Utc).AddTicks(5637));

            migrationBuilder.UpdateData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 42, 23, 669, DateTimeKind.Utc).AddTicks(5638));

            migrationBuilder.UpdateData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 42, 23, 669, DateTimeKind.Utc).AddTicks(5639));
        }
    }
}
