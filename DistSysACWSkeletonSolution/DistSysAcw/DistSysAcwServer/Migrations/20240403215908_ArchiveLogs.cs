using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DistSysAcwServer.Migrations
{
    public partial class ArchiveLogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LogArchives",
                columns: table => new
                {
                    LogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LogString = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserApiKey = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogArchives", x => x.LogId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogArchives");
        }
    }
}
