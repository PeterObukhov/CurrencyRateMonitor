using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CurrencyRateMonitor.Migrations
{
    /// <inheritdoc />
    public partial class AddedIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CurrencyRates_CurrencyId_Date",
                table: "CurrencyRates",
                columns: new[] { "CurrencyId", "Date" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CurrencyRates_CurrencyId_Date",
                table: "CurrencyRates");
        }
    }
}
