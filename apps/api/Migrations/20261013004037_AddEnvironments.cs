using System;
using System.Collections.Generic;
using System.Text.Json;
using API.Models;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class AddEnvironments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EnvironmentId",
                table: "Servers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Environments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Version = table.Column<string>(type: "text", nullable: false),
                    Author = table.Column<EnvironmentAuthor>(type: "jsonb", nullable: false),
                    Images = table.Column<Dictionary<string, string>>(type: "jsonb", nullable: false),
                    Files = table.Column<List<string>>(type: "text[]", nullable: false),
                    Startup = table.Column<EnvironmentStartup>(type: "jsonb", nullable: false),
                    Parameters = table.Column<Dictionary<string, EnvironmentParameter>>(type: "jsonb", nullable: false),
                    Variables = table.Column<Dictionary<string, JsonElement>>(type: "jsonb", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Environments", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Servers_EnvironmentId",
                table: "Servers",
                column: "EnvironmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Servers_Environments_EnvironmentId",
                table: "Servers",
                column: "EnvironmentId",
                principalTable: "Environments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Servers_Environments_EnvironmentId",
                table: "Servers");

            migrationBuilder.DropTable(
                name: "Environments");

            migrationBuilder.DropIndex(
                name: "IX_Servers_EnvironmentId",
                table: "Servers");

            migrationBuilder.DropColumn(
                name: "EnvironmentId",
                table: "Servers");
        }
    }
}
