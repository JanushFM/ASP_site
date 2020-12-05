﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Movie",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(maxLength: 60, nullable: false),
                    ReleaseDate = table.Column<DateTime>(nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Genre = table.Column<string>(maxLength: 30, nullable: false),
                    Rating = table.Column<string>(maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movie", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Movie",
                columns: new[] { "Id", "Genre", "Price", "Rating", "ReleaseDate", "Title" },
                values: new object[] { 1, "Romantic Comedy", 7.99m, "R", new DateTime(1989, 2, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "When Harry Met Sally" });

            migrationBuilder.InsertData(
                table: "Movie",
                columns: new[] { "Id", "Genre", "Price", "Rating", "ReleaseDate", "Title" },
                values: new object[] { 2, "Comedy", 8.99m, "R", new DateTime(1984, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ghostbusters " });

            migrationBuilder.InsertData(
                table: "Movie",
                columns: new[] { "Id", "Genre", "Price", "Rating", "ReleaseDate", "Title" },
                values: new object[] { 3, "Comedy", 9.99m, "R", new DateTime(1986, 2, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ghostbusters 2" });

            migrationBuilder.InsertData(
                table: "Movie",
                columns: new[] { "Id", "Genre", "Price", "Rating", "ReleaseDate", "Title" },
                values: new object[] { 4, "Western", 3.99m, "R", new DateTime(1959, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Rio Bravo" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Movie");
        }
    }
}