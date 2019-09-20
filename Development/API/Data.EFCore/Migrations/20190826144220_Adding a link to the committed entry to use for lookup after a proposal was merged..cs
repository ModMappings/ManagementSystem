using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.EFCore.Migrations
{
    public partial class Addingalinktothecommittedentrytouseforlookupafteraproposalwasmerged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassCommittedMappingEntries_ClassProposalMappingEntries_Pr~",
                table: "ClassCommittedMappingEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_FieldCommittedMappingEntries_FieldProposalMappingEntries_Pr~",
                table: "FieldCommittedMappingEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_MethodCommittedMappingEntries_MethodProposalMappingEntries_~",
                table: "MethodCommittedMappingEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_ParameterCommittedMappingEntries_ParameterProposalMappingEn~",
                table: "ParameterCommittedMappingEntries");

            migrationBuilder.DropIndex(
                name: "IX_ParameterCommittedMappingEntries_ProposalId",
                table: "ParameterCommittedMappingEntries");

            migrationBuilder.DropIndex(
                name: "IX_MethodCommittedMappingEntries_ProposalId",
                table: "MethodCommittedMappingEntries");

            migrationBuilder.DropIndex(
                name: "IX_FieldCommittedMappingEntries_ProposalId",
                table: "FieldCommittedMappingEntries");

            migrationBuilder.DropIndex(
                name: "IX_ClassCommittedMappingEntries_ProposalId",
                table: "ClassCommittedMappingEntries");

            migrationBuilder.DropColumn(
                name: "ProposalId",
                table: "ParameterCommittedMappingEntries");

            migrationBuilder.DropColumn(
                name: "ProposalId",
                table: "MethodCommittedMappingEntries");

            migrationBuilder.DropColumn(
                name: "ProposalId",
                table: "FieldCommittedMappingEntries");

            migrationBuilder.DropColumn(
                name: "ProposalId",
                table: "ClassCommittedMappingEntries");

            migrationBuilder.AddColumn<Guid>(
                name: "MergedWithId",
                table: "ParameterProposalMappingEntries",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MergedWithId",
                table: "MethodProposalMappingEntries",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MergedWithId",
                table: "FieldProposalMappingEntries",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MergedWithId",
                table: "ClassProposalMappingEntries",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParameterProposalMappingEntries_MergedWithId",
                table: "ParameterProposalMappingEntries",
                column: "MergedWithId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MethodProposalMappingEntries_MergedWithId",
                table: "MethodProposalMappingEntries",
                column: "MergedWithId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FieldProposalMappingEntries_MergedWithId",
                table: "FieldProposalMappingEntries",
                column: "MergedWithId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClassProposalMappingEntries_MergedWithId",
                table: "ClassProposalMappingEntries",
                column: "MergedWithId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassProposalMappingEntries_ClassCommittedMappingEntries_Me~",
                table: "ClassProposalMappingEntries",
                column: "MergedWithId",
                principalTable: "ClassCommittedMappingEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FieldProposalMappingEntries_FieldCommittedMappingEntries_Me~",
                table: "FieldProposalMappingEntries",
                column: "MergedWithId",
                principalTable: "FieldCommittedMappingEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MethodProposalMappingEntries_MethodCommittedMappingEntries_~",
                table: "MethodProposalMappingEntries",
                column: "MergedWithId",
                principalTable: "MethodCommittedMappingEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ParameterProposalMappingEntries_ParameterCommittedMappingEn~",
                table: "ParameterProposalMappingEntries",
                column: "MergedWithId",
                principalTable: "ParameterCommittedMappingEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassProposalMappingEntries_ClassCommittedMappingEntries_Me~",
                table: "ClassProposalMappingEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_FieldProposalMappingEntries_FieldCommittedMappingEntries_Me~",
                table: "FieldProposalMappingEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_MethodProposalMappingEntries_MethodCommittedMappingEntries_~",
                table: "MethodProposalMappingEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_ParameterProposalMappingEntries_ParameterCommittedMappingEn~",
                table: "ParameterProposalMappingEntries");

            migrationBuilder.DropIndex(
                name: "IX_ParameterProposalMappingEntries_MergedWithId",
                table: "ParameterProposalMappingEntries");

            migrationBuilder.DropIndex(
                name: "IX_MethodProposalMappingEntries_MergedWithId",
                table: "MethodProposalMappingEntries");

            migrationBuilder.DropIndex(
                name: "IX_FieldProposalMappingEntries_MergedWithId",
                table: "FieldProposalMappingEntries");

            migrationBuilder.DropIndex(
                name: "IX_ClassProposalMappingEntries_MergedWithId",
                table: "ClassProposalMappingEntries");

            migrationBuilder.DropColumn(
                name: "MergedWithId",
                table: "ParameterProposalMappingEntries");

            migrationBuilder.DropColumn(
                name: "MergedWithId",
                table: "MethodProposalMappingEntries");

            migrationBuilder.DropColumn(
                name: "MergedWithId",
                table: "FieldProposalMappingEntries");

            migrationBuilder.DropColumn(
                name: "MergedWithId",
                table: "ClassProposalMappingEntries");

            migrationBuilder.AddColumn<Guid>(
                name: "ProposalId",
                table: "ParameterCommittedMappingEntries",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProposalId",
                table: "MethodCommittedMappingEntries",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProposalId",
                table: "FieldCommittedMappingEntries",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProposalId",
                table: "ClassCommittedMappingEntries",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ParameterCommittedMappingEntries_ProposalId",
                table: "ParameterCommittedMappingEntries",
                column: "ProposalId");

            migrationBuilder.CreateIndex(
                name: "IX_MethodCommittedMappingEntries_ProposalId",
                table: "MethodCommittedMappingEntries",
                column: "ProposalId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldCommittedMappingEntries_ProposalId",
                table: "FieldCommittedMappingEntries",
                column: "ProposalId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassCommittedMappingEntries_ProposalId",
                table: "ClassCommittedMappingEntries",
                column: "ProposalId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassCommittedMappingEntries_ClassProposalMappingEntries_Pr~",
                table: "ClassCommittedMappingEntries",
                column: "ProposalId",
                principalTable: "ClassProposalMappingEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FieldCommittedMappingEntries_FieldProposalMappingEntries_Pr~",
                table: "FieldCommittedMappingEntries",
                column: "ProposalId",
                principalTable: "FieldProposalMappingEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MethodCommittedMappingEntries_MethodProposalMappingEntries_~",
                table: "MethodCommittedMappingEntries",
                column: "ProposalId",
                principalTable: "MethodProposalMappingEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ParameterCommittedMappingEntries_ParameterProposalMappingEn~",
                table: "ParameterCommittedMappingEntries",
                column: "ProposalId",
                principalTable: "ParameterProposalMappingEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
