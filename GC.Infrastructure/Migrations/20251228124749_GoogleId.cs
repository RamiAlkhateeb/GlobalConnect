using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class GoogleId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProviderLanguages",
                table: "ProviderLanguages");

            migrationBuilder.DropIndex(
                name: "IX_ProviderLanguages_ProviderId",
                table: "ProviderLanguages");

            migrationBuilder.DropColumn(
                name: "Nationality",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PreferredLanguage",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TimezoneId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Providers");

            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "Providers");

            migrationBuilder.DropColumn(
                name: "LanguageCode",
                table: "ProviderLanguages");

            migrationBuilder.DropColumn(
                name: "SlotId",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Users",
                newName: "GoogleId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ProviderLanguages",
                newName: "LanguageId");

            migrationBuilder.RenameColumn(
                name: "BookingTimeUtc",
                table: "Appointments",
                newName: "ScheduledAt");

            migrationBuilder.AddColumn<Guid>(
                name: "PublicId",
                table: "Users",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "HourlyRateUSD",
                table: "Providers",
                type: "money",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");

            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "Providers",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Nationality",
                table: "Providers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "LanguageId",
                table: "ProviderLanguages",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<string>(
                name: "MeetingLink",
                table: "Appointments",
                type: "text",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProviderLanguages",
                table: "ProviderLanguages",
                columns: new[] { "ProviderId", "LanguageId" });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    LanguageId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.LanguageId);
                });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "LanguageId", "Name" },
                values: new object[,]
                {
                    { 1, "English" },
                    { 2, "Arabic" },
                    { 3, "French" },
                    { 4, "Spanish" },
                    { 5, "German" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProviderLanguages_LanguageId",
                table: "ProviderLanguages",
                column: "LanguageId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProviderLanguages_Languages_LanguageId",
                table: "ProviderLanguages",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "LanguageId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProviderLanguages_Languages_LanguageId",
                table: "ProviderLanguages");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProviderLanguages",
                table: "ProviderLanguages");

            migrationBuilder.DropIndex(
                name: "IX_ProviderLanguages_LanguageId",
                table: "ProviderLanguages");

            migrationBuilder.DropColumn(
                name: "PublicId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Bio",
                table: "Providers");

            migrationBuilder.DropColumn(
                name: "Nationality",
                table: "Providers");

            migrationBuilder.DropColumn(
                name: "MeetingLink",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "GoogleId",
                table: "Users",
                newName: "PasswordHash");

            migrationBuilder.RenameColumn(
                name: "LanguageId",
                table: "ProviderLanguages",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ScheduledAt",
                table: "Appointments",
                newName: "BookingTimeUtc");

            migrationBuilder.AddColumn<string>(
                name: "Nationality",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PreferredLanguage",
                table: "Users",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TimezoneId",
                table: "Users",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<decimal>(
                name: "HourlyRateUSD",
                table: "Providers",
                type: "numeric(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "money");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Providers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "Providers",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ProviderLanguages",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<string>(
                name: "LanguageCode",
                table: "ProviderLanguages",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SlotId",
                table: "Appointments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProviderLanguages",
                table: "ProviderLanguages",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderLanguages_ProviderId",
                table: "ProviderLanguages",
                column: "ProviderId");
        }
    }
}
