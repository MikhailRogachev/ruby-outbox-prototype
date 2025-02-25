using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ruby_outbox_data.Persistency.Migrations
{
    public partial class updateoutboxtablelastupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedDate",
                table: "OutboxMessages",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastModifiedDate",
                table: "OutboxMessages");
        }
    }
}
