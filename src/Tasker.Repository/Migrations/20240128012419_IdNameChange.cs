using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tasker.Repository.Migrations
{
    /// <inheritdoc />
    public partial class IdNameChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "Status",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "GlobalConfigurations",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Status",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "GlobalConfigurations",
                newName: "id");
        }
    }
}
