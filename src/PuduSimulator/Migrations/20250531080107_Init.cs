using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PuduSimulator.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DiscordServers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DiscordId = table.Column<ulong>(type: "INTEGER", nullable: false),
                    ChannelId = table.Column<ulong>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordServers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pudus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DiscordServerId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pudus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pudus_DiscordServers_DiscordServerId",
                        column: x => x.DiscordServerId,
                        principalTable: "DiscordServers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServerRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReleasedPudus = table.Column<int>(type: "INTEGER", nullable: false),
                    DeadPudus = table.Column<int>(type: "INTEGER", nullable: false),
                    DiscordServerId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServerRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServerRecords_DiscordServers_DiscordServerId",
                        column: x => x.DiscordServerId,
                        principalTable: "DiscordServers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiscordServers_DiscordId",
                table: "DiscordServers",
                column: "DiscordId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pudus_DiscordServerId",
                table: "Pudus",
                column: "DiscordServerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServerRecords_DiscordServerId",
                table: "ServerRecords",
                column: "DiscordServerId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pudus");

            migrationBuilder.DropTable(
                name: "ServerRecords");

            migrationBuilder.DropTable(
                name: "DiscordServers");
        }
    }
}
