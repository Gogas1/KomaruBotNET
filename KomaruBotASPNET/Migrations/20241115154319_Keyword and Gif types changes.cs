using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KomaruBotASPNET.Migrations
{
    /// <inheritdoc />
    public partial class KeywordandGiftypeschanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Keywords",
                table: "KomaruGifs");

            migrationBuilder.CreateTable(
                name: "Keywords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Word = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GifId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Keywords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Keywords_KomaruGifs_GifId",
                        column: x => x.GifId,
                        principalTable: "KomaruGifs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Keywords_GifId",
                table: "Keywords",
                column: "GifId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Keywords");

            migrationBuilder.AddColumn<string>(
                name: "Keywords",
                table: "KomaruGifs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
