using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace triggan.BlogManager.Migrations
{
    public partial class AddBooks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShortReview = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LongReview = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateRead = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    BuyLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Books_Entity_Id",
                        column: x => x.Id,
                        principalTable: "Entity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Books");
        }
    }
}
