using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineGames.Migrations
{
    public partial class Inicijalna0 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Igrica",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(nullable: true),
                    DatumIzlaska = table.Column<DateTime>(nullable: false),
                    Izdavac = table.Column<string>(nullable: true),
                    Specifikacije = table.Column<string>(nullable: true),
                    Cijena = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Igrica", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Kategorija",
                columns: table => new
                {
                    KategorijaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kategorija", x => x.KategorijaId);
                });

            migrationBuilder.CreateTable(
                name: "KategorijaIgrica",
                columns: table => new
                {
                    KategorijaIgricaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IgricaId = table.Column<int>(nullable: false),
                    KategorijaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KategorijaIgrica", x => x.KategorijaIgricaId);
                    table.ForeignKey(
                        name: "FK_KategorijaIgrica_Igrica_IgricaId",
                        column: x => x.IgricaId,
                        principalTable: "Igrica",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KategorijaIgrica_Kategorija_KategorijaId",
                        column: x => x.KategorijaId,
                        principalTable: "Kategorija",
                        principalColumn: "KategorijaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KategorijaIgrica_IgricaId",
                table: "KategorijaIgrica",
                column: "IgricaId");

            migrationBuilder.CreateIndex(
                name: "IX_KategorijaIgrica_KategorijaId",
                table: "KategorijaIgrica",
                column: "KategorijaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KategorijaIgrica");

            migrationBuilder.DropTable(
                name: "Igrica");

            migrationBuilder.DropTable(
                name: "Kategorija");
        }
    }
}
