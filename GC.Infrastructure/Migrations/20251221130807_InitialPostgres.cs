using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialPostgres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    IsProvider = table.Column<bool>(type: "boolean", nullable: false),
                    PreferredLanguage = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    TimezoneId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Nationality = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Providers",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    PhotoUrl = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Specialty = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HourlyRateUSD = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Providers", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Providers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GeneratedSlots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProviderId = table.Column<int>(type: "integer", nullable: false),
                    SlotStartUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SlotEndUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsBooked = table.Column<bool>(type: "boolean", nullable: false)
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
                name: "ProviderLanguages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProviderId = table.Column<int>(type: "integer", nullable: false),
                    LanguageCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProviderLanguages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProviderLanguages_Providers_ProviderId",
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
                    StartTimeLocal = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTimeLocal = table.Column<TimeSpan>(type: "time", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SlotId = table.Column<int>(type: "integer", nullable: false),
                    SeekerId = table.Column<int>(type: "integer", nullable: false),
                    ProviderId = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PaymentTransactionId = table.Column<string>(type: "text", nullable: false),
                    BookingTimestampUTC = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appointments_GeneratedSlots_SlotId",
                        column: x => x.SlotId,
                        principalTable: "GeneratedSlots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Appointments_Providers_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "Providers",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Appointments_Users_SeekerId",
                        column: x => x.SeekerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_ProviderId",
                table: "Appointments",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_SeekerId",
                table: "Appointments",
                column: "SeekerId");

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
                name: "IX_ProviderLanguages_ProviderId",
                table: "ProviderLanguages",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingHours_ProviderId",
                table: "WorkingHours",
                column: "ProviderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "ProviderLanguages");

            migrationBuilder.DropTable(
                name: "WorkingHours");

            migrationBuilder.DropTable(
                name: "GeneratedSlots");

            migrationBuilder.DropTable(
                name: "Providers");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
