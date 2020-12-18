using Microsoft.EntityFrameworkCore.Migrations;

namespace Mayor.Data.Migrations
{
    public partial class ChangesToPictureModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Pictures_PictureId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_Issues_TitlePictureId",
                table: "Issues");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PictureId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PictureId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "AddedByUserId",
                table: "Pictures",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IssueId",
                table: "Pictures",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_AddedByUserId",
                table: "Pictures",
                column: "AddedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Issues_TitlePictureId",
                table: "Issues",
                column: "TitlePictureId",
                unique: true,
                filter: "[TitlePictureId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Pictures_AspNetUsers_AddedByUserId",
                table: "Pictures",
                column: "AddedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pictures_AspNetUsers_AddedByUserId",
                table: "Pictures");

            migrationBuilder.DropIndex(
                name: "IX_Pictures_AddedByUserId",
                table: "Pictures");

            migrationBuilder.DropIndex(
                name: "IX_Issues_TitlePictureId",
                table: "Issues");

            migrationBuilder.DropColumn(
                name: "AddedByUserId",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "IssueId",
                table: "Pictures");

            migrationBuilder.AddColumn<string>(
                name: "PictureId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Issues_TitlePictureId",
                table: "Issues",
                column: "TitlePictureId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PictureId",
                table: "AspNetUsers",
                column: "PictureId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Pictures_PictureId",
                table: "AspNetUsers",
                column: "PictureId",
                principalTable: "Pictures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
