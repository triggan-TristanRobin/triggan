using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace triggan.BlogManager.Migrations
{
    public partial class AddUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Update_Entity_ProjectId",
                table: "Update");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "Entity");

            migrationBuilder.DropColumn(
                name: "CoverImagePath",
                table: "Entity");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Entity");

            migrationBuilder.DropColumn(
                name: "Excerpt",
                table: "Entity");

            migrationBuilder.DropColumn(
                name: "LastUpdate",
                table: "Entity");

            migrationBuilder.DropColumn(
                name: "Project_CoverImagePath",
                table: "Entity");

            migrationBuilder.DropColumn(
                name: "Project_Excerpt",
                table: "Entity");

            migrationBuilder.DropColumn(
                name: "Project_Public",
                table: "Entity");

            migrationBuilder.DropColumn(
                name: "Project_PublicationDate",
                table: "Entity");

            migrationBuilder.DropColumn(
                name: "Project_Title",
                table: "Entity");

            migrationBuilder.DropColumn(
                name: "Project_Views",
                table: "Entity");

            migrationBuilder.DropColumn(
                name: "Public",
                table: "Entity");

            migrationBuilder.DropColumn(
                name: "PublicationDate",
                table: "Entity");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Entity");

            migrationBuilder.DropColumn(
                name: "Tags",
                table: "Entity");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Entity");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Entity");

            migrationBuilder.DropColumn(
                name: "Views",
                table: "Entity");

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Views = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Excerpt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CoverImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Public = table.Column<bool>(type: "bit", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    PublicationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_Entity_Id",
                        column: x => x.Id,
                        principalTable: "Entity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Views = table.Column<int>(type: "int", nullable: false),
                    Excerpt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CoverImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Public = table.Column<bool>(type: "bit", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    PublicationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_Entity_Id",
                        column: x => x.Id,
                        principalTable: "Entity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Entity_Id",
                        column: x => x.Id,
                        principalTable: "Entity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Update_Projects_ProjectId",
                table: "Update",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Update_Projects_ProjectId",
                table: "Update");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Entity",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoverImagePath",
                table: "Entity",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Entity",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Excerpt",
                table: "Entity",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdate",
                table: "Entity",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Project_CoverImagePath",
                table: "Entity",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Project_Excerpt",
                table: "Entity",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Project_Public",
                table: "Entity",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Project_PublicationDate",
                table: "Entity",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Project_Title",
                table: "Entity",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Project_Views",
                table: "Entity",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Public",
                table: "Entity",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublicationDate",
                table: "Entity",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Entity",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tags",
                table: "Entity",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Entity",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Entity",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Views",
                table: "Entity",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Update_Entity_ProjectId",
                table: "Update",
                column: "ProjectId",
                principalTable: "Entity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
