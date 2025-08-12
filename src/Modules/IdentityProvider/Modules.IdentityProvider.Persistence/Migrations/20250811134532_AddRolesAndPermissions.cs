using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.IdentityProvider.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddRolesAndPermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                INSERT INTO 
                users.""AspNetRoles""(""Id"", ""Name"", ""NormalizedName"", ""ConcurrencyStamp"")
                VALUES
                ('f34c63b8-d59b-4cf7-bf30-d568103a5d2d', 'admin', 'ADMIN', null),
                ('d43be063-6398-44c2-9c13-9fcad7e84128', 'member', 'MEMBER', null),
                ('0a4851a6-0860-4060-b9b6-6c1eea344f0f', 'manager', 'MANAGER', null);");

            migrationBuilder.Sql(@"
                INSERT INTO 
                users.""AspNetRoleClaims""(""RoleId"", ""ClaimType"", ""ClaimValue"")
                VALUES
                ('f34c63b8-d59b-4cf7-bf30-d568103a5d2d', 'Permission', 'CreatePendingMember'),
                ('f34c63b8-d59b-4cf7-bf30-d568103a5d2d', 'Permission', 'ReadPendingMember'),
                ('f34c63b8-d59b-4cf7-bf30-d568103a5d2d', 'Permission', 'DeletePendingMember'),
                ('f34c63b8-d59b-4cf7-bf30-d568103a5d2d', 'Permission', 'ReadMember'),
                ('f34c63b8-d59b-4cf7-bf30-d568103a5d2d', 'Permission', 'UpdateMember'),
                ('f34c63b8-d59b-4cf7-bf30-d568103a5d2d', 'Permission', 'DeleteMember'),
                ('f34c63b8-d59b-4cf7-bf30-d568103a5d2d', 'Permission', 'UpdateProfile'),
                ('f34c63b8-d59b-4cf7-bf30-d568103a5d2d', 'Permission', 'ReadProfile'),
                ('f34c63b8-d59b-4cf7-bf30-d568103a5d2d', 'Permission', 'CreateMassLocation'),
                ('f34c63b8-d59b-4cf7-bf30-d568103a5d2d', 'Permission', 'ReadMassLocation'),
                ('f34c63b8-d59b-4cf7-bf30-d568103a5d2d', 'Permission', 'UpdateMassLocation'),
                ('f34c63b8-d59b-4cf7-bf30-d568103a5d2d', 'Permission', 'DeleteMassLocation'),
                ('f34c63b8-d59b-4cf7-bf30-d568103a5d2d', 'Permission', 'CreateOtherSchedule'),
                ('f34c63b8-d59b-4cf7-bf30-d568103a5d2d', 'Permission', 'ReadOtherSchedule'),
                ('f34c63b8-d59b-4cf7-bf30-d568103a5d2d', 'Permission', 'UpdateOtherSchedule'),
                ('f34c63b8-d59b-4cf7-bf30-d568103a5d2d', 'Permission', 'DeleteOtherSchedule');");

            migrationBuilder.Sql(@"
                INSERT INTO 
                users.""AspNetRoleClaims""(""RoleId"", ""ClaimType"", ""ClaimValue"")
                VALUES
                ('0a4851a6-0860-4060-b9b6-6c1eea344f0f', 'Permission', 'UpdateProfile'),
                ('0a4851a6-0860-4060-b9b6-6c1eea344f0f', 'Permission', 'ReadProfile'),
                ('0a4851a6-0860-4060-b9b6-6c1eea344f0f', 'Permission', 'CreateMassLocation'),
                ('0a4851a6-0860-4060-b9b6-6c1eea344f0f', 'Permission', 'ReadMassLocation'),
                ('0a4851a6-0860-4060-b9b6-6c1eea344f0f', 'Permission', 'UpdateMassLocation'),
                ('0a4851a6-0860-4060-b9b6-6c1eea344f0f', 'Permission', 'CreateOtherSchedule'),
                ('0a4851a6-0860-4060-b9b6-6c1eea344f0f', 'Permission', 'ReadOtherSchedule'),
                ('0a4851a6-0860-4060-b9b6-6c1eea344f0f', 'Permission', 'UpdateOtherSchedule'),
                ('0a4851a6-0860-4060-b9b6-6c1eea344f0f', 'Permission', 'DeleteOtherSchedule');");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
