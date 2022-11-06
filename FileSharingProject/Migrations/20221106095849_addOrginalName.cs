using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileSharingProject.Migrations
{
    public partial class addOrginalName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrginalName",
                table: "Uploads",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrginalName",
                table: "Uploads");
        }
    }
}
