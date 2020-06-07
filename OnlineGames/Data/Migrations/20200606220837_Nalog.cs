using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineGames.Data.Migrations
{
    public partial class Nalog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Nalog",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    NalogId = table.Column<int>(nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                    KorisnikId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nalog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Nalog_AspNetUsers_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Nalog_KorisnikId",
                table: "Nalog",
                column: "KorisnikId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Nalog");
        }
    }
}
