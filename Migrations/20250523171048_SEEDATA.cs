using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace apiWebpractice.Migrations
{
    /// <inheritdoc />
    public partial class SEEDATA : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_VideoGames",
                table: "VideoGames");

            migrationBuilder.RenameTable(
                name: "VideoGames",
                newName: "videoGames");

            migrationBuilder.AddPrimaryKey(
                name: "PK_videoGames",
                table: "videoGames",
                column: "Id");

            migrationBuilder.InsertData(
                table: "videoGames",
                columns: new[] { "Id", "Genre", "Name", "ReleaseYear" },
                values: new object[,]
                {
                    { 1, "Action-adventure", "The Legend of Zelda: Breath of the Wild", 2017 },
                    { 2, "Action role-playing", "The Witcher 3: Wild Hunt", 2015 },
                    { 3, "Action role-playing", "Dark Souls III", 2016 },
                    { 4, "Action-adventure", "God of War", 2018 },
                    { 5, "Action-adventure", "Red Dead Redemption 2", 2018 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_videoGames",
                table: "videoGames");

            migrationBuilder.DeleteData(
                table: "videoGames",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "videoGames",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "videoGames",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "videoGames",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "videoGames",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.RenameTable(
                name: "videoGames",
                newName: "VideoGames");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VideoGames",
                table: "VideoGames",
                column: "Id");
        }
    }
}
