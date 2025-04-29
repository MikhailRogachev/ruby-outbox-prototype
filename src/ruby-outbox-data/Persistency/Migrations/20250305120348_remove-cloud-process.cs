using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ruby_outbox_data.Persistency.Migrations
{
    public partial class removecloudprocess : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CloudProcesses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_outboxErrorLoggers",
                table: "outboxErrorLoggers");

            migrationBuilder.RenameTable(
                name: "outboxErrorLoggers",
                newName: "OutboxErrorLoggers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OutboxErrorLoggers",
                table: "OutboxErrorLoggers",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OutboxErrorLoggers",
                table: "OutboxErrorLoggers");

            migrationBuilder.RenameTable(
                name: "OutboxErrorLoggers",
                newName: "outboxErrorLoggers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_outboxErrorLoggers",
                table: "outboxErrorLoggers",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "CloudProcesses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VmId = table.Column<Guid>(type: "uuid", nullable: true),
                    Comment = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CloudProcesses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CloudProcesses_Vms_VmId",
                        column: x => x.VmId,
                        principalTable: "Vms",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CloudProcesses_VmId",
                table: "CloudProcesses",
                column: "VmId");
        }
    }
}
