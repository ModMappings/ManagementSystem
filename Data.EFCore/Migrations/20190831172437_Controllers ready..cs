using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.EFCore.Migrations
{
    public partial class Controllersready : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FieldVersionedMappings_ClassCommittedMappingEntries_MemberO~",
                table: "FieldVersionedMappings");

            migrationBuilder.DropForeignKey(
                name: "FK_MethodVersionedMappings_ClassCommittedMappingEntries_Member~",
                table: "MethodVersionedMappings");

            migrationBuilder.DropForeignKey(
                name: "FK_ParameterCommittedMappingEntries_MethodVersionedMappings_Me~",
                table: "ParameterCommittedMappingEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_ParameterVersionedMappings_MethodCommittedMappingEntries_Pa~",
                table: "ParameterVersionedMappings");

            migrationBuilder.DropIndex(
                name: "IX_ParameterCommittedMappingEntries_MethodVersionedMappingId",
                table: "ParameterCommittedMappingEntries");

            migrationBuilder.DropColumn(
                name: "Documentation",
                table: "ParameterMappings");

            migrationBuilder.DropColumn(
                name: "MethodVersionedMappingId",
                table: "ParameterCommittedMappingEntries");

            migrationBuilder.DropColumn(
                name: "Documentation",
                table: "MethodMappings");

            migrationBuilder.DropColumn(
                name: "Documentation",
                table: "FieldMappings");

            migrationBuilder.DropColumn(
                name: "Documentation",
                table: "ClassMappings");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "ParameterProposalMappingEntries",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Documentation",
                table: "ParameterProposalMappingEntries",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "ParameterCommittedMappingEntries",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Documentation",
                table: "ParameterCommittedMappingEntries",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "MethodProposalMappingEntries",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Documentation",
                table: "MethodProposalMappingEntries",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "MethodCommittedMappingEntries",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Documentation",
                table: "MethodCommittedMappingEntries",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsStatic",
                table: "FieldVersionedMappings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "FieldProposalMappingEntries",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Documentation",
                table: "FieldProposalMappingEntries",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "FieldCommittedMappingEntries",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Documentation",
                table: "FieldCommittedMappingEntries",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "ClassProposalMappingEntries",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Documentation",
                table: "ClassProposalMappingEntries",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "ClassCommittedMappingEntries",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Documentation",
                table: "ClassCommittedMappingEntries",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FieldVersionedMappings_ClassVersionedMappings_MemberOfId",
                table: "FieldVersionedMappings",
                column: "MemberOfId",
                principalTable: "ClassVersionedMappings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MethodVersionedMappings_ClassVersionedMappings_MemberOfId",
                table: "MethodVersionedMappings",
                column: "MemberOfId",
                principalTable: "ClassVersionedMappings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ParameterVersionedMappings_MethodVersionedMappings_Paramete~",
                table: "ParameterVersionedMappings",
                column: "ParameterOfId",
                principalTable: "MethodVersionedMappings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FieldVersionedMappings_ClassVersionedMappings_MemberOfId",
                table: "FieldVersionedMappings");

            migrationBuilder.DropForeignKey(
                name: "FK_MethodVersionedMappings_ClassVersionedMappings_MemberOfId",
                table: "MethodVersionedMappings");

            migrationBuilder.DropForeignKey(
                name: "FK_ParameterVersionedMappings_MethodVersionedMappings_Paramete~",
                table: "ParameterVersionedMappings");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "ParameterProposalMappingEntries");

            migrationBuilder.DropColumn(
                name: "Documentation",
                table: "ParameterProposalMappingEntries");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "ParameterCommittedMappingEntries");

            migrationBuilder.DropColumn(
                name: "Documentation",
                table: "ParameterCommittedMappingEntries");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "MethodProposalMappingEntries");

            migrationBuilder.DropColumn(
                name: "Documentation",
                table: "MethodProposalMappingEntries");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "MethodCommittedMappingEntries");

            migrationBuilder.DropColumn(
                name: "Documentation",
                table: "MethodCommittedMappingEntries");

            migrationBuilder.DropColumn(
                name: "IsStatic",
                table: "FieldVersionedMappings");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "FieldProposalMappingEntries");

            migrationBuilder.DropColumn(
                name: "Documentation",
                table: "FieldProposalMappingEntries");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "FieldCommittedMappingEntries");

            migrationBuilder.DropColumn(
                name: "Documentation",
                table: "FieldCommittedMappingEntries");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "ClassProposalMappingEntries");

            migrationBuilder.DropColumn(
                name: "Documentation",
                table: "ClassProposalMappingEntries");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "ClassCommittedMappingEntries");

            migrationBuilder.DropColumn(
                name: "Documentation",
                table: "ClassCommittedMappingEntries");

            migrationBuilder.AddColumn<string>(
                name: "Documentation",
                table: "ParameterMappings",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MethodVersionedMappingId",
                table: "ParameterCommittedMappingEntries",
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

            migrationBuilder.CreateIndex(
                name: "IX_ParameterCommittedMappingEntries_MethodVersionedMappingId",
                table: "ParameterCommittedMappingEntries",
                column: "MethodVersionedMappingId");

            migrationBuilder.AddForeignKey(
                name: "FK_FieldVersionedMappings_ClassCommittedMappingEntries_MemberO~",
                table: "FieldVersionedMappings",
                column: "MemberOfId",
                principalTable: "ClassCommittedMappingEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MethodVersionedMappings_ClassCommittedMappingEntries_Member~",
                table: "MethodVersionedMappings",
                column: "MemberOfId",
                principalTable: "ClassCommittedMappingEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ParameterCommittedMappingEntries_MethodVersionedMappings_Me~",
                table: "ParameterCommittedMappingEntries",
                column: "MethodVersionedMappingId",
                principalTable: "MethodVersionedMappings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ParameterVersionedMappings_MethodCommittedMappingEntries_Pa~",
                table: "ParameterVersionedMappings",
                column: "ParameterOfId",
                principalTable: "MethodCommittedMappingEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
