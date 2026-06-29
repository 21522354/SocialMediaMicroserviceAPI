using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoryService.Migrations
{
    /// <inheritdoc />
    public partial class RefactorUserIdToInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [UserAlreadySeenStories]");
            migrationBuilder.Sql("DELETE FROM [Stories]");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAlreadySeenStories",
                table: "UserAlreadySeenStories");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "UserAlreadySeenStories",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Stories",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAlreadySeenStories",
                table: "UserAlreadySeenStories",
                columns: new[] { "StoryId", "UserId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [UserAlreadySeenStories]");
            migrationBuilder.Sql("DELETE FROM [Stories]");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAlreadySeenStories",
                table: "UserAlreadySeenStories");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "UserAlreadySeenStories",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Stories",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAlreadySeenStories",
                table: "UserAlreadySeenStories",
                columns: new[] { "StoryId", "UserId" });
        }
    }
}
