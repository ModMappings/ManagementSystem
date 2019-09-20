using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.EFCore.Migrations
{
    public partial class RemovalofthepossibilitytochangepackageandorparentclassofaclassusingproposalAddedpackagenametotheversionedclass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassCommittedMappingEntries_ClassCommittedMappingEntries_P~",
                table: "ClassCommittedMappingEntries");

            migrationBuilder.DropIndex(
                name: "IX_ClassCommittedMappingEntries_ParentId",
                table: "ClassCommittedMappingEntries");

            migrationBuilder.DropColumn(
                name: "Package",
                table: "ClassCommittedMappingEntries");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "ClassCommittedMappingEntries");

            migrationBuilder.AddColumn<string>(
                name: "Package",
                table: "ClassVersionedMappings",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                table: "ClassVersionedMappings",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClassVersionedMappings_ParentId",
                table: "ClassVersionedMappings",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassVersionedMappings_ClassVersionedMappings_ParentId",
                table: "ClassVersionedMappings",
                column: "ParentId",
                principalTable: "ClassVersionedMappings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassVersionedMappings_ClassVersionedMappings_ParentId",
                table: "ClassVersionedMappings");

            migrationBuilder.DropIndex(
                name: "IX_ClassVersionedMappings_ParentId",
                table: "ClassVersionedMappings");

            migrationBuilder.DropColumn(
                name: "Package",
                table: "ClassVersionedMappings");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "ClassVersionedMappings");

            migrationBuilder.AddColumn<string>(
                name: "Package",
                table: "ClassCommittedMappingEntries",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                table: "ClassCommittedMappingEntries",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClassCommittedMappingEntries_ParentId",
                table: "ClassCommittedMappingEntries",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassCommittedMappingEntries_ClassCommittedMappingEntries_P~",
                table: "ClassCommittedMappingEntries",
                column: "ParentId",
                principalTable: "ClassCommittedMappingEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
