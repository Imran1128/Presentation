using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Presentation.Migrations
{
    /// <inheritdoc />
    public partial class sdf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "presentationsModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PresentationName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PresentationDetails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Photo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_presentationsModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ViewerModel",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Id1 = table.Column<int>(type: "int", nullable: false),
                    isViewer = table.Column<bool>(type: "bit", nullable: false),
                    isEditor = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ViewerModel", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_ViewerModel_presentationsModel_Id1",
                        column: x => x.Id1,
                        principalTable: "presentationsModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ViewerModel_Id1",
                table: "ViewerModel",
                column: "Id1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ViewerModel");

            migrationBuilder.DropTable(
                name: "presentationsModel");
        }
    }
}
