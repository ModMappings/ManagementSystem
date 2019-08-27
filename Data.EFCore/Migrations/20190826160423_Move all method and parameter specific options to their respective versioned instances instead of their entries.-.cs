using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.EFCore.Migrations
{
    public partial class Moveallmethodandparameterspecificoptionstotheirrespectiveversionedinstancesinsteadoftheirentries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParameterVersionedMappings_MethodCommittedMappingEntries_Pa~",
                table: "ParameterVersionedMappings");

            migrationBuilder.DropColumn(
                name: "Index",
                table: "ParameterProposalMappingEntries");

            migrationBuilder.DropColumn(
                name: "Index",
                table: "ParameterCommittedMappingEntries");

            migrationBuilder.DropColumn(
                name: "IsStatic",
                table: "MethodProposalMappingEntries");

            migrationBuilder.DropColumn(
                name: "IsStatic",
                table: "MethodCommittedMappingEntries");

            migrationBuilder.AlterColumn<Guid>(
                name: "ParameterOfId",
                table: "ParameterVersionedMappings",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Index",
                table: "ParameterVersionedMappings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsStatic",
                table: "MethodVersionedMappings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_ParameterVersionedMappings_MethodCommittedMappingEntries_Pa~",
                table: "ParameterVersionedMappings",
                column: "ParameterOfId",
                principalTable: "MethodCommittedMappingEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParameterVersionedMappings_MethodCommittedMappingEntries_Pa~",
                table: "ParameterVersionedMappings");

            migrationBuilder.DropColumn(
                name: "Index",
                table: "ParameterVersionedMappings");

            migrationBuilder.DropColumn(
                name: "IsStatic",
                table: "MethodVersionedMappings");

            migrationBuilder.AlterColumn<Guid>(
                name: "ParameterOfId",
                table: "ParameterVersionedMappings",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<int>(
                name: "Index",
                table: "ParameterProposalMappingEntries",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Index",
                table: "ParameterCommittedMappingEntries",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsStatic",
                table: "MethodProposalMappingEntries",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsStatic",
                table: "MethodCommittedMappingEntries",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_ParameterVersionedMappings_MethodCommittedMappingEntries_Pa~",
                table: "ParameterVersionedMappings",
                column: "ParameterOfId",
                principalTable: "MethodCommittedMappingEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
