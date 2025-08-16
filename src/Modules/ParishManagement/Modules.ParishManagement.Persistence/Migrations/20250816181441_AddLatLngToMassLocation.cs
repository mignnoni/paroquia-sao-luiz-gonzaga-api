using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.ParishManagement.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddLatLngToMassLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "latitude",
                schema: "parishmanagement",
                table: "mass_location",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "longitude",
                schema: "parishmanagement",
                table: "mass_location",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "latitude",
                schema: "parishmanagement",
                table: "mass_location");

            migrationBuilder.DropColumn(
                name: "longitude",
                schema: "parishmanagement",
                table: "mass_location");
        }
    }
}
