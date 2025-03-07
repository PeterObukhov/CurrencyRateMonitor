using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CurrencyRateMonitor.Migrations
{
    /// <inheritdoc />
    public partial class CurrencyCodesNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CurrencyRates_CurrencyCode_CurrencyCodeId",
                table: "CurrencyRates");

            migrationBuilder.DropIndex(
                name: "IX_CurrencyRates_CurrencyCodeId",
                table: "CurrencyRates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CurrencyCode",
                table: "CurrencyCode");

            migrationBuilder.DropColumn(
                name: "CurrencyCodeId",
                table: "CurrencyRates");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CurrencyCode");

            migrationBuilder.RenameTable(
                name: "CurrencyCode",
                newName: "CurrencyCodes");

            migrationBuilder.RenameIndex(
                name: "IX_CurrencyCode_Code",
                table: "CurrencyCodes",
                newName: "IX_CurrencyCodes_Code");

            migrationBuilder.AddColumn<string>(
                name: "CurrencyCodeCode",
                table: "CurrencyRates",
                type: "text",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CurrencyCodes",
                table: "CurrencyCodes",
                column: "Code");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CurrencyRates_CurrencyCodes_CurrencyCodeCode",
                table: "CurrencyRates");

            migrationBuilder.DropIndex(
                name: "IX_CurrencyRates_CurrencyCodeCode",
                table: "CurrencyRates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CurrencyCodes",
                table: "CurrencyCodes");

            migrationBuilder.DropColumn(
                name: "CurrencyCodeCode",
                table: "CurrencyRates");

            migrationBuilder.RenameTable(
                name: "CurrencyCodes",
                newName: "CurrencyCode");

            migrationBuilder.RenameIndex(
                name: "IX_CurrencyCodes_Code",
                table: "CurrencyCode",
                newName: "IX_CurrencyCode_Code");

            migrationBuilder.AddColumn<int>(
                name: "CurrencyCodeId",
                table: "CurrencyRates",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "CurrencyCode",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CurrencyCode",
                table: "CurrencyCode",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyRates_CurrencyCodeId",
                table: "CurrencyRates",
                column: "CurrencyCodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CurrencyRates_CurrencyCode_CurrencyCodeId",
                table: "CurrencyRates",
                column: "CurrencyCodeId",
                principalTable: "CurrencyCode",
                principalColumn: "Id");
        }
    }
}
