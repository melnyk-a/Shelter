using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shelter.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updatedRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_permissions_role_role_id",
                table: "permissions");

            migrationBuilder.DropIndex(
                name: "ix_permissions_role_id",
                table: "permissions");

            migrationBuilder.DropColumn(
                name: "role_id",
                table: "permissions");

            migrationBuilder.CreateIndex(
                name: "ix_role_permisions_permission_id",
                table: "role_permisions",
                column: "permission_id");

            migrationBuilder.AddForeignKey(
                name: "fk_role_permisions_permissions_permission_id",
                table: "role_permisions",
                column: "permission_id",
                principalTable: "permissions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_role_permisions_roles_role_id",
                table: "role_permisions",
                column: "role_id",
                principalTable: "roles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_role_permisions_permissions_permission_id",
                table: "role_permisions");

            migrationBuilder.DropForeignKey(
                name: "fk_role_permisions_roles_role_id",
                table: "role_permisions");

            migrationBuilder.DropIndex(
                name: "ix_role_permisions_permission_id",
                table: "role_permisions");

            migrationBuilder.AddColumn<int>(
                name: "role_id",
                table: "permissions",
                type: "integer",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 1,
                column: "role_id",
                value: null);

            migrationBuilder.CreateIndex(
                name: "ix_permissions_role_id",
                table: "permissions",
                column: "role_id");

            migrationBuilder.AddForeignKey(
                name: "fk_permissions_role_role_id",
                table: "permissions",
                column: "role_id",
                principalTable: "roles",
                principalColumn: "id");
        }
    }
}
