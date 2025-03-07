using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CurrencyRateMonitor.Migrations
{
    /// <inheritdoc />
    public partial class CurrencyCodes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrencyName",
                table: "CurrencyRates");

            migrationBuilder.AddColumn<int>(
                name: "CurrencyCodeId",
                table: "CurrencyRates",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CurrencyCode",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyCode", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyRates_CurrencyCodeId",
                table: "CurrencyRates",
                column: "CurrencyCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyCode_Code",
                table: "CurrencyCode",
                column: "Code",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CurrencyRates_CurrencyCode_CurrencyCodeId",
                table: "CurrencyRates",
                column: "CurrencyCodeId",
                principalTable: "CurrencyCode",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CurrencyRates_CurrencyCode_CurrencyCodeId",
                table: "CurrencyRates");

            migrationBuilder.DropTable(
                name: "CurrencyCode");

            migrationBuilder.DropIndex(
                name: "IX_CurrencyRates_CurrencyCodeId",
                table: "CurrencyRates");

            migrationBuilder.DropColumn(
                name: "CurrencyCodeId",
                table: "CurrencyRates");

            migrationBuilder.AddColumn<string>(
                name: "CurrencyName",
                table: "CurrencyRates",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
