using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketStack.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tag_name",
                table: "tag");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_tag_name",
                table: "tag",
                column: "name",
                unique: true);
        }
    }
}
