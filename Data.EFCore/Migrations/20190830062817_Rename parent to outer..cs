using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.EFCore.Migrations
{
    public partial class Renameparenttoouter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassVersionedMappings_ClassVersionedMappings_ParentId",
                table: "ClassVersionedMappings");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "ClassVersionedMappings",
                newName: "OuterId");

            migrationBuilder.RenameIndex(
                name: "IX_ClassVersionedMappings_ParentId",
                table: "ClassVersionedMappings",
                newName: "IX_ClassVersionedMappings_OuterId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassVersionedMappings_ClassVersionedMappings_OuterId",
                table: "ClassVersionedMappings",
                column: "OuterId",
                principalTable: "ClassVersionedMappings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassVersionedMappings_ClassVersionedMappings_OuterId",
                table: "ClassVersionedMappings");

            migrationBuilder.RenameColumn(
                name: "OuterId",
                table: "ClassVersionedMappings",
                newName: "ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_ClassVersionedMappings_OuterId",
                table: "ClassVersionedMappings",
                newName: "IX_ClassVersionedMappings_ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassVersionedMappings_ClassVersionedMappings_ParentId",
                table: "ClassVersionedMappings",
                column: "ParentId",
                principalTable: "ClassVersionedMappings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
