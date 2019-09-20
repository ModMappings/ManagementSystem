using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.EFCore.Migrations
{
    public partial class MCPInitializationsupportready : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Releases_ClassCommittedMappingEntries_ClassCommittedMapping~",
                table: "Releases");

            migrationBuilder.DropForeignKey(
                name: "FK_Releases_FieldCommittedMappingEntries_FieldCommittedMapping~",
                table: "Releases");

            migrationBuilder.DropForeignKey(
                name: "FK_Releases_MethodCommittedMappingEntries_MethodCommittedMappi~",
                table: "Releases");

            migrationBuilder.DropForeignKey(
                name: "FK_Releases_ParameterCommittedMappingEntries_ParameterCommitte~",
                table: "Releases");

            migrationBuilder.DropIndex(
                name: "IX_Releases_ClassCommittedMappingEntryId",
                table: "Releases");

            migrationBuilder.DropIndex(
                name: "IX_Releases_FieldCommittedMappingEntryId",
                table: "Releases");

            migrationBuilder.DropIndex(
                name: "IX_Releases_MethodCommittedMappingEntryId",
                table: "Releases");

            migrationBuilder.DropIndex(
                name: "IX_Releases_ParameterCommittedMappingEntryId",
                table: "Releases");

            migrationBuilder.DropColumn(
                name: "ClassCommittedMappingEntryId",
                table: "Releases");

            migrationBuilder.DropColumn(
                name: "FieldCommittedMappingEntryId",
                table: "Releases");

            migrationBuilder.DropColumn(
                name: "MethodCommittedMappingEntryId",
                table: "Releases");

            migrationBuilder.DropColumn(
                name: "ParameterCommittedMappingEntryId",
                table: "Releases");

            migrationBuilder.CreateIndex(
                name: "IX_Releases_Name",
                table: "Releases",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameVersions_Name",
                table: "GameVersions",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Releases_Name",
                table: "Releases");

            migrationBuilder.DropIndex(
                name: "IX_GameVersions_Name",
                table: "GameVersions");

            migrationBuilder.AddColumn<Guid>(
                name: "ClassCommittedMappingEntryId",
                table: "Releases",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FieldCommittedMappingEntryId",
                table: "Releases",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MethodCommittedMappingEntryId",
                table: "Releases",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ParameterCommittedMappingEntryId",
                table: "Releases",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Releases_ClassCommittedMappingEntryId",
                table: "Releases",
                column: "ClassCommittedMappingEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_Releases_FieldCommittedMappingEntryId",
                table: "Releases",
                column: "FieldCommittedMappingEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_Releases_MethodCommittedMappingEntryId",
                table: "Releases",
                column: "MethodCommittedMappingEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_Releases_ParameterCommittedMappingEntryId",
                table: "Releases",
                column: "ParameterCommittedMappingEntryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Releases_ClassCommittedMappingEntries_ClassCommittedMapping~",
                table: "Releases",
                column: "ClassCommittedMappingEntryId",
                principalTable: "ClassCommittedMappingEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Releases_FieldCommittedMappingEntries_FieldCommittedMapping~",
                table: "Releases",
                column: "FieldCommittedMappingEntryId",
                principalTable: "FieldCommittedMappingEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Releases_MethodCommittedMappingEntries_MethodCommittedMappi~",
                table: "Releases",
                column: "MethodCommittedMappingEntryId",
                principalTable: "MethodCommittedMappingEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Releases_ParameterCommittedMappingEntries_ParameterCommitte~",
                table: "Releases",
                column: "ParameterCommittedMappingEntryId",
                principalTable: "ParameterCommittedMappingEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
