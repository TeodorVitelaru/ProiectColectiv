using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatingApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMatchEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    MatchedUserId = table.Column<long>(type: "bigint", nullable: false),
                    IsMutual = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Matches_Users_MatchedUserId",
                        column: x => x.MatchedUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Matches_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Match_UserIds_Unique",
                table: "Matches",
                columns: new[] { "UserId", "MatchedUserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Matches_MatchedUserId",
                table: "Matches",
                column: "MatchedUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Matches");
        }
    }
}
