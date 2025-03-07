using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CurrencyRateMonitor.Migrations
{
    /// <inheritdoc />
    public partial class ForeignKeyFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CurrencyRates_CurrencyCodes_CurrencyCodeCode",
                table: "CurrencyRates");

            migrationBuilder.DropIndex(
                name: "IX_CurrencyRates_CurrencyCodeCode",
                table: "CurrencyRates");

            migrationBuilder.DropColumn(
                name: "CurrencyCodeCode",
                table: "CurrencyRates");

            migrationBuilder.AddForeignKey(
                name: "FK_CurrencyRates_CurrencyCodes_CurrencyId",
                table: "CurrencyRates",
                column: "CurrencyId",
                principalTable: "CurrencyCodes",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CurrencyRates_CurrencyCodes_CurrencyId",
                table: "CurrencyRates");

            migrationBuilder.AddColumn<string>(
                name: "CurrencyCodeCode",
                table: "CurrencyRates",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyRates_CurrencyCodeCode",
                table: "CurrencyRates",
                column: "CurrencyCodeCode");

            migrationBuilder.AddForeignKey(
                name: "FK_CurrencyRates_CurrencyCodes_CurrencyCodeCode",
                table: "CurrencyRates",
                column: "CurrencyCodeCode",
                principalTable: "CurrencyCodes",
                principalColumn: "Code");
        }
    }
}
