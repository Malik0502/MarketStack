using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MarketStack.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedPriceSummaryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "receipt_price_summary",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    receipt_id = table.Column<int>(type: "integer", nullable: false),
                    vat_rate = table.Column<int>(type: "integer", nullable: false),
                    tax_inclusive_price = table.Column<decimal>(type: "numeric", nullable: false),
                    tax_amount = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_receipt_price_summary", x => x.id);
                    table.ForeignKey(
                        name: "FK_receipt_price_summary_receipt_receipt_id",
                        column: x => x.receipt_id,
                        principalTable: "receipt",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_receipt_price_summary_receipt_id",
                table: "receipt_price_summary",
                column: "receipt_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "receipt_price_summary");
        }
    }
}
