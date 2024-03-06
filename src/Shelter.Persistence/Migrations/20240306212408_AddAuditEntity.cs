using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shelter.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "reviews",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_date",
                table: "reviews",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "reviews",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "reviews",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "bookings",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_date",
                table: "bookings",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "bookings",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "bookings",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "created_by",
                table: "reviews");

            migrationBuilder.DropColumn(
                name: "created_date",
                table: "reviews");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "reviews");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "reviews");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "bookings");

            migrationBuilder.DropColumn(
                name: "created_date",
                table: "bookings");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "bookings");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "bookings");
        }
    }
}
