using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dotnet_github_webhook.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OptionChange",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FileName = table.Column<string>(type: "VARCHAR2", maxLength: 64, nullable: true),
                    From = table.Column<string>(type: "VARCHAR2", maxLength: 64, nullable: true),
                    To = table.Column<string>(type: "VARCHAR2", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OptionChange", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "OptionVersion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Tag = table.Column<string>(type: "VARCHAR2", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OptionVersion", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OptionChange");

            migrationBuilder.DropTable(
                name: "OptionVersion");
        }
    }
}
