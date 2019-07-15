using Microsoft.EntityFrameworkCore.Migrations;

namespace LinkManager.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorie",
                columns: table => new
                {
                    IdCategoria = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(nullable: true),
                    Descrizione = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorie", x => x.IdCategoria);
                });

            migrationBuilder.CreateTable(
                name: "Links",
                columns: table => new
                {
                    IdLink = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Titolo = table.Column<string>(nullable: true),
                    Descrizione = table.Column<string>(nullable: true),
                    URL = table.Column<string>(nullable: true),
                    IdCategoria = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Links", x => x.IdLink);
                    table.ForeignKey(
                        name: "FK_Links_Categorie_IdCategoria",
                        column: x => x.IdCategoria,
                        principalTable: "Categorie",
                        principalColumn: "IdCategoria",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Links_IdCategoria",
                table: "Links",
                column: "IdCategoria");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Links");

            migrationBuilder.DropTable(
                name: "Categorie");
        }
    }
}
