using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.EFCore.Migrations
{
    public partial class AddingsupportforclosingofproposalsandmarkingpropertiesasRequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOpen",
                table: "ParameterProposalMappingEntries",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsOpen",
                table: "MethodProposalMappingEntries",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsOpen",
                table: "FieldProposalMappingEntries",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsOpen",
                table: "ClassProposalMappingEntries",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOpen",
                table: "ParameterProposalMappingEntries");

            migrationBuilder.DropColumn(
                name: "IsOpen",
                table: "MethodProposalMappingEntries");

            migrationBuilder.DropColumn(
                name: "IsOpen",
                table: "FieldProposalMappingEntries");

            migrationBuilder.DropColumn(
                name: "IsOpen",
                table: "ClassProposalMappingEntries");
        }
    }
}
