using Microsoft.EntityFrameworkCore.Migrations;

namespace JetsonWeb.Migrations
{
    public partial class InitialCreate : Migration
    {
        /// <summary>
        /// Creates the Utilization table and configured Id as the primary key.
        /// </summary>
        /// <param name="migrationBuilder"></param>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Utilization",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilization", x => x.Id);
                });
        }

        /// <summary>
        /// Reverts the schema changes made up the <see cref="Up(MigrationBuilder)"></see> migration.
        /// </summary>
        /// <param name="migrationBuilder"></param>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Utilization");
        }
    }
}
