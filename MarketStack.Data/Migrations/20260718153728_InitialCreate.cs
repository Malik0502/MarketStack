using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MarketStack.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "product",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "store_chain",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_store_chain", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "store_location",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    store_chain_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    street = table.Column<string>(type: "text", nullable: false),
                    city = table.Column<string>(type: "text", nullable: false),
                    postal_code = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_store_location", x => x.id);
                    table.ForeignKey(
                        name: "FK_store_location_store_chain_store_chain_id",
                        column: x => x.store_chain_id,
                        principalTable: "store_chain",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "receipt",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    store_location_id = table.Column<int>(type: "integer", nullable: false),
                    purchasedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_receipt", x => x.id);
                    table.ForeignKey(
                        name: "FK_receipt_store_location_store_location_id",
                        column: x => x.store_location_id,
                        principalTable: "store_location",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "receipt_item",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    receipt_id = table.Column<int>(type: "integer", nullable: false),
                    product_id = table.Column<int>(type: "integer", nullable: false),
                    quantity = table.Column<decimal>(type: "numeric", nullable: false),
                    net_price = table.Column<decimal>(type: "numeric", nullable: false),
                    gross_price = table.Column<decimal>(type: "numeric", nullable: false),
                    vat_rate = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_receipt_item", x => x.id);
                    table.ForeignKey(
                        name: "FK_receipt_item_product_product_id",
                        column: x => x.product_id,
                        principalTable: "product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_receipt_item_receipt_receipt_id",
                        column: x => x.receipt_id,
                        principalTable: "receipt",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "receipt_total",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    net_total = table.Column<decimal>(type: "numeric", nullable: false),
                    vat_total = table.Column<decimal>(type: "numeric", nullable: false),
                    vat_type_A_total = table.Column<decimal>(type: "numeric", nullable: false),
                    vat_type_B_total = table.Column<decimal>(type: "numeric", nullable: false),
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

            migrationBuilder.CreateIndex(
                name: "IX_receipt_store_location_id",
                table: "receipt",
                column: "store_location_id");

            migrationBuilder.CreateIndex(
                name: "IX_receipt_item_product_id",
                table: "receipt_item",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_receipt_item_receipt_id",
                table: "receipt_item",
                column: "receipt_id");

            migrationBuilder.CreateIndex(
                name: "IX_store_location_store_chain_id",
                table: "store_location",
                column: "store_chain_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "receipt_item");

            migrationBuilder.DropTable(
                name: "receipt_total");

            migrationBuilder.DropTable(
                name: "product");

            migrationBuilder.DropTable(
                name: "receipt");

            migrationBuilder.DropTable(
                name: "store_location");

            migrationBuilder.DropTable(
                name: "store_chain");
        }
    }
}
