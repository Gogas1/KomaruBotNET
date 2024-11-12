using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KomaruBotASPNET.Migrations
{
    /// <inheritdoc />
    public partial class FileType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddKomaruFlow_IsSticker",
                table: "UserInputStates");

            migrationBuilder.DropColumn(
                name: "IsSticker",
                table: "KomaruGifs");

            migrationBuilder.AddColumn<int>(
                name: "AddKomaruFlow_FileType",
                table: "UserInputStates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FileType",
                table: "KomaruGifs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddKomaruFlow_FileType",
                table: "UserInputStates");

            migrationBuilder.DropColumn(
                name: "FileType",
                table: "KomaruGifs");

            migrationBuilder.AddColumn<bool>(
                name: "AddKomaruFlow_IsSticker",
                table: "UserInputStates",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSticker",
                table: "KomaruGifs",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
