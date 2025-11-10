using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatingApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameMessageSenderRecipient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Messages_UserId1",
                table: "Messages",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_UserId2",
                table: "Messages",
                column: "UserId2");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_UserId1",
                table: "Messages",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_UserId2",
                table: "Messages",
                column: "UserId2",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_UserId1",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_UserId2",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_UserId1",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_UserId2",
                table: "Messages");
        }
    }
}
