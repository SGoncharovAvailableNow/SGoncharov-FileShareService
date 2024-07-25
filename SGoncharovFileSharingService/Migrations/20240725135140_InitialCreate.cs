using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SGoncharovFileSharingService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FileUUID = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FileEntities",
                columns: table => new
                {
                    Uuid = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    FilePath = table.Column<string>(type: "text", nullable: false),
                    DeletePassword = table.Column<string>(type: "text", nullable: false),
                    FileUUID = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileEntities", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_FileEntities_UserEntities_FileUUID",
                        column: x => x.FileUUID,
                        principalTable: "UserEntities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FileEntities_UserEntities_UserId",
                        column: x => x.UserId,
                        principalTable: "UserEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileEntities_FileUUID",
                table: "FileEntities",
                column: "FileUUID");

            migrationBuilder.CreateIndex(
                name: "IX_FileEntities_UserId",
                table: "FileEntities",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileEntities");

            migrationBuilder.DropTable(
                name: "UserEntities");
        }
    }
}
