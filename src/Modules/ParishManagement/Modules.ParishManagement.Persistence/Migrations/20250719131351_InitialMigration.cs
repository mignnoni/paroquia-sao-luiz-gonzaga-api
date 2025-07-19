using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.ParishManagement.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "parishmanagement");

            migrationBuilder.CreateTable(
                name: "mass_location",
                schema: "parishmanagement",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    address = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    is_headquarters = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mass_location", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "member",
                schema: "parishmanagement",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    full_name = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    email = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_member", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "other_schedule",
                schema: "parishmanagement",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_other_schedule", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "mass_schedule",
                schema: "parishmanagement",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    mass_location_id = table.Column<Guid>(type: "uuid", nullable: false),
                    day = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mass_schedule", x => x.id);
                    table.ForeignKey(
                        name: "fk_mass_schedule_mass_location_mass_location_id",
                        column: x => x.mass_location_id,
                        principalSchema: "parishmanagement",
                        principalTable: "mass_location",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mass_time",
                schema: "parishmanagement",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    mass_schedule_id = table.Column<Guid>(type: "uuid", nullable: false),
                    time = table.Column<TimeOnly>(type: "time", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mass_time", x => x.id);
                    table.ForeignKey(
                        name: "fk_mass_time_mass_schedule_mass_schedule_id",
                        column: x => x.mass_schedule_id,
                        principalSchema: "parishmanagement",
                        principalTable: "mass_schedule",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_mass_schedule_mass_location_id",
                schema: "parishmanagement",
                table: "mass_schedule",
                column: "mass_location_id");

            migrationBuilder.CreateIndex(
                name: "ix_mass_time_mass_schedule_id",
                schema: "parishmanagement",
                table: "mass_time",
                column: "mass_schedule_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "mass_time",
                schema: "parishmanagement");

            migrationBuilder.DropTable(
                name: "member",
                schema: "parishmanagement");

            migrationBuilder.DropTable(
                name: "other_schedule",
                schema: "parishmanagement");

            migrationBuilder.DropTable(
                name: "mass_schedule",
                schema: "parishmanagement");

            migrationBuilder.DropTable(
                name: "mass_location",
                schema: "parishmanagement");
        }
    }
}
