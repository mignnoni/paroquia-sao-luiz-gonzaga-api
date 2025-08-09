using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.ParishManagement.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddOtherScheduleFiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "other_schedule_file",
                schema: "parishmanagement",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    other_schedule_id = table.Column<Guid>(type: "uuid", nullable: false),
                    upload_info_content_type = table.Column<string>(type: "text", nullable: false),
                    upload_info_file_name = table.Column<string>(type: "text", nullable: false),
                    upload_info_title = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "other_schedule_file",
                schema: "parishmanagement");
        }
    }
}
