using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDifficultyEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "BookingStatuses",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 28, 0, 43, 40, 581, DateTimeKind.Utc).AddTicks(8912));

            migrationBuilder.UpdateData(
                table: "BookingStatuses",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 28, 0, 43, 40, 581, DateTimeKind.Utc).AddTicks(8915));

            migrationBuilder.UpdateData(
                table: "BookingStatuses",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 28, 0, 43, 40, 581, DateTimeKind.Utc).AddTicks(8916));

            migrationBuilder.UpdateData(
                table: "BookingStatuses",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 28, 0, 43, 40, 581, DateTimeKind.Utc).AddTicks(8917));

            migrationBuilder.InsertData(
                table: "Difficulties",
                columns: new[] { "Id", "CreatedAt", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 6, 28, 0, 43, 40, 582, DateTimeKind.Utc).AddTicks(533), "Easy" },
                    { 2, new DateTime(2026, 6, 28, 0, 43, 40, 582, DateTimeKind.Utc).AddTicks(535), "Medium" },
                    { 3, new DateTime(2026, 6, 28, 0, 43, 40, 582, DateTimeKind.Utc).AddTicks(536), "Hard" },
                    { 4, new DateTime(2026, 6, 28, 0, 43, 40, 582, DateTimeKind.Utc).AddTicks(537), "Expert" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: 4);

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
    }
}
