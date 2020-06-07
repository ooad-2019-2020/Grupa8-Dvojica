using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineGames.Migrations
{
    public partial class Slika : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SlikaIgriceId",
                table: "Igrica",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SlikaIgirce",
                columns: table => new
                {
                    SlikaIgriceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageTitle = table.Column<string>(nullable: true),
                    ImageData = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SlikaIgirce", x => x.SlikaIgriceId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Igrica_SlikaIgriceId",
                table: "Igrica",
                column: "SlikaIgriceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Igrica_SlikaIgirce_SlikaIgriceId",
                table: "Igrica",
                column: "SlikaIgriceId",
                principalTable: "SlikaIgirce",
                principalColumn: "SlikaIgriceId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Igrica_SlikaIgirce_SlikaIgriceId",
                table: "Igrica");

            migrationBuilder.DropTable(
                name: "SlikaIgirce");

            migrationBuilder.DropIndex(
                name: "IX_Igrica_SlikaIgriceId",
                table: "Igrica");

            migrationBuilder.DropColumn(
                name: "SlikaIgriceId",
                table: "Igrica");
        }
    }
}
