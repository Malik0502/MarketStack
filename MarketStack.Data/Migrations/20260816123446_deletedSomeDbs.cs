using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MarketStack.Data.Migrations
{
    /// <inheritdoc />
    public partial class deletedSomeDbs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_receipt_store_location_store_location_id",
                table: "receipt");

            migrationBuilder.DropTable(
                name: "store_location");

            migrationBuilder.DropTable(
                name: "store_chain");

            migrationBuilder.DropIndex(
                name: "IX_receipt_store_location_id",
                table: "receipt");

            migrationBuilder.DropColumn(
                name: "net_total",
                table: "receipt_total");

            migrationBuilder.DropColumn(
                name: "vat_total",
                table: "receipt_total");

            migrationBuilder.DropColumn(
                name: "vat_type_A_total",
                table: "receipt_total");

            migrationBuilder.DropColumn(
                name: "vat_type_B_total",
                table: "receipt_total");

            migrationBuilder.DropColumn(
                name: "gross_price",
                table: "receipt_item");

            migrationBuilder.DropColumn(
                name: "store_location_id",
                table: "receipt");

            migrationBuilder.RenameColumn(
                name: "net_price",
                table: "receipt_item",
                newName: "price");

            migrationBuilder.AlterColumn<int>(
                name: "vat_rate",
                table: "receipt_item",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AddColumn<string>(
                name: "promotion_id",
                table: "receipt_item",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "store_intern_item_id",
                table: "receipt_item",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "store",
                table: "receipt",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ticket_id",
                table: "receipt",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "promotion_id",
                table: "receipt_item");

            migrationBuilder.DropColumn(
                name: "store_intern_item_id",
                table: "receipt_item");

            migrationBuilder.DropColumn(
                name: "store",
                table: "receipt");

            migrationBuilder.DropColumn(
                name: "ticket_id",
                table: "receipt");

            migrationBuilder.RenameColumn(
                name: "price",
                table: "receipt_item",
                newName: "net_price");

            migrationBuilder.AddColumn<decimal>(
                name: "net_total",
                table: "receipt_total",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "vat_total",
                table: "receipt_total",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "vat_type_A_total",
                table: "receipt_total",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "vat_type_B_total",
                table: "receipt_total",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "vat_rate",
                table: "receipt_item",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<decimal>(
                name: "gross_price",
                table: "receipt_item",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "store_location_id",
                table: "receipt",
                type: "integer",
                nullable: false,
                defaultValue: 0);

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
                    city = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    postal_code = table.Column<string>(type: "text", nullable: false),
                    street = table.Column<string>(type: "text", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_receipt_store_location_id",
                table: "receipt",
                column: "store_location_id");

            migrationBuilder.CreateIndex(
                name: "IX_store_location_store_chain_id",
                table: "store_location",
                column: "store_chain_id");

            migrationBuilder.AddForeignKey(
                name: "FK_receipt_store_location_store_location_id",
                table: "receipt",
                column: "store_location_id",
                principalTable: "store_location",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
