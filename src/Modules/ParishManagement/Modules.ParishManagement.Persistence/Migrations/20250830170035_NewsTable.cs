using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.ParishManagement.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class NewsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "other_schedule_file",
                schema: "parishmanagement");

            migrationBuilder.CreateTable(
                name: "news",
                schema: "parishmanagement",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    summary = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    highlight = table.Column<bool>(type: "boolean", nullable: false),
                    highlight_until = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_news", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "news_file",
                schema: "parishmanagement",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    news_id = table.Column<Guid>(type: "uuid", nullable: false),
                    upload_info_content_type = table.Column<string>(type: "text", nullable: false),
                    upload_info_file_name = table.Column<string>(type: "text", nullable: false),
                    upload_info_title = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_news_file", x => x.id);
                    table.ForeignKey(
                        name: "fk_news_file_news_news_id",
                        column: x => x.news_id,
                        principalSchema: "parishmanagement",
                        principalTable: "news",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_news_file_news_id",
                schema: "parishmanagement",
                table: "news_file",
                column: "news_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "news_file",
                schema: "parishmanagement");

            migrationBuilder.DropTable(
                name: "news",
                schema: "parishmanagement");

            migrationBuilder.CreateTable(
                name: "other_schedule_file",
                schema: "parishmanagement",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    other_schedule_id = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    upload_info_content_type = table.Column<string>(type: "text", nullable: false),
                    upload_info_file_name = table.Column<string>(type: "text", nullable: false),
                    upload_info_title = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_other_schedule_file", x => x.id);
                    table.ForeignKey(
                        name: "fk_other_schedule_file_other_schedule_other_schedule_id",
                        column: x => x.other_schedule_id,
                        principalSchema: "parishmanagement",
                        principalTable: "other_schedule",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_other_schedule_file_other_schedule_id",
                schema: "parishmanagement",
                table: "other_schedule_file",
                column: "other_schedule_id");
        }
    }
}
