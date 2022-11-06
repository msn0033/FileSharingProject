using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileSharingProject.Migrations
{
    public partial class addUploadColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Size",
                table: "Uploads",
                type: "decimal(18,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<DateTime>(
                name: "UploadDate",
                table: "Uploads",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getDate()");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UploadDate",
                table: "Uploads");

            migrationBuilder.AlterColumn<decimal>(
                name: "Size",
                table: "Uploads",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)");
        }
    }
}
