using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.EFCore.Migrations
{
    public partial class Removalofmappingtype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InputMappingType",
                table: "ParameterProposalMappingEntries");

            migrationBuilder.DropColumn(
                name: "OutputMappingType",
                table: "ParameterProposalMappingEntries");

            migrationBuilder.DropColumn(
                name: "InputMappingType",
                table: "ParameterCommittedMappingEntries");

            migrationBuilder.DropColumn(
                name: "OutputMappingType",
                table: "ParameterCommittedMappingEntries");

            migrationBuilder.DropColumn(
                name: "InputMappingType",
                table: "MethodProposalMappingEntries");

            migrationBuilder.DropColumn(
                name: "OutputMappingType",
                table: "MethodProposalMappingEntries");

            migrationBuilder.DropColumn(
                name: "InputMappingType",
                table: "MethodCommittedMappingEntries");

            migrationBuilder.DropColumn(
                name: "OutputMappingType",
                table: "MethodCommittedMappingEntries");

            migrationBuilder.DropColumn(
                name: "InputMappingType",
                table: "FieldProposalMappingEntries");

            migrationBuilder.DropColumn(
                name: "OutputMappingType",
                table: "FieldProposalMappingEntries");

            migrationBuilder.DropColumn(
                name: "InputMappingType",
                table: "FieldCommittedMappingEntries");

            migrationBuilder.DropColumn(
                name: "OutputMappingType",
                table: "FieldCommittedMappingEntries");

            migrationBuilder.DropColumn(
                name: "InputMappingType",
                table: "ClassProposalMappingEntries");

            migrationBuilder.DropColumn(
                name: "OutputMappingType",
                table: "ClassProposalMappingEntries");

            migrationBuilder.DropColumn(
                name: "InputMappingType",
                table: "ClassCommittedMappingEntries");

            migrationBuilder.DropColumn(
                name: "OutputMappingType",
                table: "ClassCommittedMappingEntries");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InputMappingType",
                table: "ParameterProposalMappingEntries",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OutputMappingType",
                table: "ParameterProposalMappingEntries",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InputMappingType",
                table: "ParameterCommittedMappingEntries",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OutputMappingType",
                table: "ParameterCommittedMappingEntries",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InputMappingType",
                table: "MethodProposalMappingEntries",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OutputMappingType",
                table: "MethodProposalMappingEntries",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InputMappingType",
                table: "MethodCommittedMappingEntries",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OutputMappingType",
                table: "MethodCommittedMappingEntries",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InputMappingType",
                table: "FieldProposalMappingEntries",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OutputMappingType",
                table: "FieldProposalMappingEntries",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InputMappingType",
                table: "FieldCommittedMappingEntries",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OutputMappingType",
                table: "FieldCommittedMappingEntries",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InputMappingType",
                table: "ClassProposalMappingEntries",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OutputMappingType",
                table: "ClassProposalMappingEntries",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InputMappingType",
                table: "ClassCommittedMappingEntries",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OutputMappingType",
                table: "ClassCommittedMappingEntries",
                nullable: false,
                defaultValue: "");
        }
    }
}
