using Microsoft.EntityFrameworkCore.Migrations;

namespace API_Article.Migrations
{
    public partial class article_createdbyId_UP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Articles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedByUserId",
                table: "Articles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Articles_CreatedById",
                table: "Articles",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Users_CreatedById",
                table: "Articles",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Users_CreatedById",
                table: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_Articles_CreatedById",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Articles");
        }
    }
}
