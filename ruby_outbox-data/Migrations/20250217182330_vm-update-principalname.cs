using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ruby_outbox_data.Migrations
{
    public partial class vmupdateprincipalname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PrincipalName",
                table: "Vms",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrincipalName",
                table: "Vms");
        }
    }
}
