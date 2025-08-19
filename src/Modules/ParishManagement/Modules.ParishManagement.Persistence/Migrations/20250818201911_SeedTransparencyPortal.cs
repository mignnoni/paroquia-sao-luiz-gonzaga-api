using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.ParishManagement.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedTransparencyPortal : Migration
    {
        private readonly Guid _transparencyPortalId = Guid.NewGuid();
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "other_schedule",
                columns: ["id", "title", "content", "type", "created_at"],
                values: [_transparencyPortalId, "Portal de transparência", string.Empty, 3, DateTime.UtcNow]
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "other_schedule",
                keyColumn: "id",
                keyValue: _transparencyPortalId
            );
        }
    }
}
