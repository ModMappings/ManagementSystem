using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.EFCore.Migrations
{
    public partial class Addsupportforinheritanceanddocumentation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Documentation",
                table: "ParameterMappings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Documentation",
                table: "MethodMappings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Documentation",
                table: "FieldMappings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Documentation",
                table: "ClassMappings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Documentation",
                table: "ParameterMappings");

            migrationBuilder.DropColumn(
                name: "Documentation",
                table: "MethodMappings");

            migrationBuilder.DropColumn(
                name: "Documentation",
                table: "FieldMappings");

            migrationBuilder.DropColumn(
                name: "Documentation",
                table: "ClassMappings");
        }
    }
}
