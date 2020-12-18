using Microsoft.EntityFrameworkCore.Migrations;

namespace Mayor.Data.Migrations
{
    public partial class EditIssueAndPictureModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Issues_TitlePictureId",
                table: "Issues");

            migrationBuilder.AlterColumn<string>(
                name: "TitlePictureId",
                table: "Issues",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Issues_TitlePictureId",
                table: "Issues",
                column: "TitlePictureId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Issues_TitlePictureId",
                table: "Issues");

            migrationBuilder.AlterColumn<string>(
                name: "TitlePictureId",
                table: "Issues",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Issues_TitlePictureId",
                table: "Issues",
                column: "TitlePictureId",
                unique: true,
                filter: "[TitlePictureId] IS NOT NULL");
        }
    }
}
