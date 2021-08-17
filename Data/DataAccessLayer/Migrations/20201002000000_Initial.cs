using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Entity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: false),
                    Slug = table.Column<string>(nullable: true),
                    Deleted = table.Column<bool>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Views = table.Column<int>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    Excerpt = table.Column<string>(nullable: true),
                    CoverImagePath = table.Column<string>(nullable: true),
                    Public = table.Column<bool>(nullable: true),
                    PublicationDate = table.Column<DateTime>(nullable: true),
                    Tags = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entity", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Entity_Slug",
                table: "Entity",
                column: "Slug",
                unique: true,
                filter: "[Slug] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Entity");
        }
    }
}
