using Microsoft.EntityFrameworkCore.Migrations;

namespace Mayor.Data.Migrations
{
    public partial class AlterPictureAndIssueRelationToOneToMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issues_Pictures_TitlePictureId",
                table: "Issues");

            migrationBuilder.DropIndex(
                name: "IX_Issues_TitlePictureId",
                table: "Issues");

            migrationBuilder.DropColumn(
                name: "TitlePictureId",
                table: "Issues");

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_IssueId",
                table: "Pictures",
                column: "IssueId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pictures_Issues_IssueId",
                table: "Pictures",
                column: "IssueId",
                principalTable: "Issues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pictures_Issues_IssueId",
                table: "Pictures");

            migrationBuilder.DropIndex(
                name: "IX_Pictures_IssueId",
                table: "Pictures");

            migrationBuilder.AddColumn<string>(
                name: "TitlePictureId",
                table: "Issues",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Issues_TitlePictureId",
                table: "Issues",
                column: "TitlePictureId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_Pictures_TitlePictureId",
                table: "Issues",
                column: "TitlePictureId",
                principalTable: "Pictures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
