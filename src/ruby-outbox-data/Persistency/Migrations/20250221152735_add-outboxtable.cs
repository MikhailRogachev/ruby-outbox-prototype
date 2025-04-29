using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ruby_outbox_data.Persistency.Migrations
{
    public partial class addoutboxtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vms_Customer_CustomerId",
                table: "Vms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Customer",
                table: "Customer");

            migrationBuilder.RenameTable(
                name: "Customer",
                newName: "Customers");

            migrationBuilder.AlterColumn<Guid>(
                name: "VmId",
                table: "CloudProcesses",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customers",
                table: "Customers",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "OutboxMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<JsonDocument>(type: "jsonb", nullable: true),
                    ContentType = table.Column<string>(type: "text", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessages", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CloudProcesses_VmId",
                table: "CloudProcesses",
                column: "VmId");

            migrationBuilder.AddForeignKey(
                name: "FK_CloudProcesses_Vms_VmId",
                table: "CloudProcesses",
                column: "VmId",
                principalTable: "Vms",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vms_Customers_CustomerId",
                table: "Vms",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CloudProcesses_Vms_VmId",
                table: "CloudProcesses");

            migrationBuilder.DropForeignKey(
                name: "FK_Vms_Customers_CustomerId",
                table: "Vms");

            migrationBuilder.DropTable(
                name: "OutboxMessages");

            migrationBuilder.DropIndex(
                name: "IX_CloudProcesses_VmId",
                table: "CloudProcesses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Customers",
                table: "Customers");

            migrationBuilder.RenameTable(
                name: "Customers",
                newName: "Customer");

            migrationBuilder.AlterColumn<Guid>(
                name: "VmId",
                table: "CloudProcesses",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customer",
                table: "Customer",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vms_Customer_CustomerId",
                table: "Vms",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id");
        }
    }
}
