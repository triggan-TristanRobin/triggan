﻿using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace triggan.BlogManager.Migrations
{
    public partial class AddProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "Entity",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Entity",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Project_CoverImagePath",
                table: "Entity",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Project_Excerpt",
                table: "Entity",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Project_Public",
                table: "Entity",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Project_PublicationDate",
                table: "Entity",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Entity",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Project_Title",
                table: "Entity",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Project_Views",
                table: "Entity",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Entity_ProjectId",
                table: "Entity",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Entity_Entity_ProjectId",
                table: "Entity",
                column: "ProjectId",
                principalTable: "Entity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entity_Entity_ProjectId",
                table: "Entity");

            migrationBuilder.DropIndex(
                name: "IX_Entity_ProjectId",
                table: "Entity");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Entity");

            migrationBuilder.DropColumn(
                name: "Type",
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
                name: "State",
                table: "Entity");

            migrationBuilder.DropColumn(
                name: "Project_Title",
                table: "Entity");

            migrationBuilder.DropColumn(
                name: "Project_Views",
                table: "Entity");
        }
    }
}