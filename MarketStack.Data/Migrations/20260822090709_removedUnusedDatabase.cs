using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketStack.Data.Migrations
{
    /// <inheritdoc />
    public partial class removedUnusedDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "receipt_total");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "receipt_total",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    gros_total = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_receipt_total", x => x.id);
                    table.ForeignKey(
                        name: "FK_receipt_total_receipt_id",
                        column: x => x.id,
                        principalTable: "receipt",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });
        }
    }
}
