using Microsoft.EntityFrameworkCore.Migrations;

namespace Mayor.Data.Migrations
{
    public partial class FixIssueTitlePictureIdColType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issues_Pictures_TitlePictureId1",
                table: "Issues");

            migrationBuilder.DropIndex(
                name: "IX_Issues_TitlePictureId1",
                table: "Issues");

            migrationBuilder.DropColumn(
                name: "TitlePictureId1",
                table: "Issues");

            migrationBuilder.AlterColumn<string>(
                name: "TitlePictureId",
                table: "Issues",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Issues_TitlePictureId",
                table: "Issues",
                column: "TitlePictureId");

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_Pictures_TitlePictureId",
                table: "Issues",
                column: "TitlePictureId",
                principalTable: "Pictures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issues_Pictures_TitlePictureId",
                table: "Issues");

            migrationBuilder.DropIndex(
                name: "IX_Issues_TitlePictureId",
                table: "Issues");

            migrationBuilder.AlterColumn<int>(
                name: "TitlePictureId",
                table: "Issues",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TitlePictureId1",
                table: "Issues",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Issues_TitlePictureId1",
                table: "Issues",
                column: "TitlePictureId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_Pictures_TitlePictureId1",
                table: "Issues",
                column: "TitlePictureId1",
                principalTable: "Pictures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
