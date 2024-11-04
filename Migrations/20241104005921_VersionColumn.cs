using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dotnet_github_webhook.Migrations
{
    /// <inheritdoc />
    public partial class VersionColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "OptionChange",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "Version",
                table: "OptionChange",
                type: "VARCHAR2",
                maxLength: 64,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                table: "OptionChange");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "OptionChange",
                newName: "id");
        }
    }
}
