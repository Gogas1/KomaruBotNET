using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KomaruBotASPNET.Migrations
{
    /// <inheritdoc />
    public partial class Changes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AddKomaruFlow_FileId",
                table: "UserInputStates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "AddKomaruFlow_IsSticker",
                table: "UserInputStates",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "AddKomaruFlow_Keywords",
                table: "UserInputStates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<string>(
                name: "AddKomaruFlow_Name",
                table: "UserInputStates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsSticker",
                table: "KomaruGifs",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddKomaruFlow_FileId",
                table: "UserInputStates");

            migrationBuilder.DropColumn(
                name: "AddKomaruFlow_IsSticker",
                table: "UserInputStates");

            migrationBuilder.DropColumn(
                name: "AddKomaruFlow_Keywords",
                table: "UserInputStates");

            migrationBuilder.DropColumn(
                name: "AddKomaruFlow_Name",
                table: "UserInputStates");

            migrationBuilder.DropColumn(
                name: "IsSticker",
                table: "KomaruGifs");
        }
    }
}
