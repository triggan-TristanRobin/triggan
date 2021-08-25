using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace triggan.BlogManager.Migrations
{
    public partial class AddProjectLastUpdateProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdate",
                table: "Entity",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastUpdate",
                table: "Entity");
        }
    }
}