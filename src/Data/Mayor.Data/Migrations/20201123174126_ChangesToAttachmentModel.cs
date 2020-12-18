using Microsoft.EntityFrameworkCore.Migrations;

namespace Mayor.Data.Migrations
{
    public partial class ChangesToAttachmentModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AddedByUserId",
                table: "Pictures",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddedByUserId",
                table: "Attachments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_AddedByUserId",
                table: "Attachments",
                column: "AddedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_AspNetUsers_AddedByUserId",
                table: "Attachments",
                column: "AddedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_AspNetUsers_AddedByUserId",
                table: "Attachments");

            migrationBuilder.DropIndex(
                name: "IX_Attachments_AddedByUserId",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "AddedByUserId",
                table: "Attachments");

            migrationBuilder.AlterColumn<string>(
                name: "AddedByUserId",
                table: "Pictures",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
