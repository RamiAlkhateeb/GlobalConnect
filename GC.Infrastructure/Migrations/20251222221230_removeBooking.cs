using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removeBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_GeneratedSlots_SlotId",
                table: "Appointments");

            migrationBuilder.DropTable(
                name: "GeneratedSlots");

            migrationBuilder.DropTable(
                name: "WorkingHours");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_SlotId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "BookingTimestampUTC",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "PaymentTransactionId",
                table: "Appointments",
                newName: "GoogleEventId");

            migrationBuilder.AddColumn<string>(
                name: "GoogleBookingUrl",
                table: "Providers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GoogleCalendarId",
                table: "Providers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GoogleRefreshToken",
                table: "Providers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "BookingTimeUtc",
                table: "Appointments",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GoogleBookingUrl",
                table: "Providers");

            migrationBuilder.DropColumn(
                name: "GoogleCalendarId",
                table: "Providers");

            migrationBuilder.DropColumn(
                name: "GoogleRefreshToken",
                table: "Providers");

            migrationBuilder.DropColumn(
                name: "BookingTimeUtc",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "GoogleEventId",
                table: "Appointments",
                newName: "PaymentTransactionId");

            migrationBuilder.AddColumn<DateTime>(
                name: "BookingTimestampUTC",
                table: "Appointments",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "GeneratedSlots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProviderId = table.Column<int>(type: "integer", nullable: false),
                    IsBooked = table.Column<bool>(type: "boolean", nullable: false),
                    SlotEndUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SlotStartUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneratedSlots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneratedSlots_Providers_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "Providers",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkingHours",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProviderId = table.Column<int>(type: "integer", nullable: false),
                    DayOfWeek = table.Column<int>(type: "integer", nullable: false),
                    EndTimeLocal = table.Column<TimeSpan>(type: "time", nullable: false),
                    StartTimeLocal = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingHours", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkingHours_Providers_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "Providers",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_SlotId",
                table: "Appointments",
                column: "SlotId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GeneratedSlots_ProviderId_SlotStartUTC",
                table: "GeneratedSlots",
                columns: new[] { "ProviderId", "SlotStartUTC" });

            migrationBuilder.CreateIndex(
                name: "IX_WorkingHours_ProviderId",
                table: "WorkingHours",
                column: "ProviderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_GeneratedSlots_SlotId",
                table: "Appointments",
                column: "SlotId",
                principalTable: "GeneratedSlots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
