using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PendingChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFavoriteCurrencies_Currencies_CurrencyId",
                table: "UserFavoriteCurrencies");

            migrationBuilder.DropIndex(
                name: "IX_UserFavoriteCurrencies_CurrencyId",
                table: "UserFavoriteCurrencies");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserFavoriteCurrencies_CurrencyId",
                table: "UserFavoriteCurrencies",
                column: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFavoriteCurrencies_Currencies_CurrencyId",
                table: "UserFavoriteCurrencies",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
