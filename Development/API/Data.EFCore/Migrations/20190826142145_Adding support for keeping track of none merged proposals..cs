using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.EFCore.Migrations
{
    public partial class Addingsupportforkeepingtrackofnonemergedproposals : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassProposalMappingEntries_Users_CommittedById",
                table: "ClassProposalMappingEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_FieldProposalMappingEntries_Users_CommittedById",
                table: "FieldProposalMappingEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_MethodProposalMappingEntries_Users_CommittedById",
                table: "MethodProposalMappingEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_ParameterProposalMappingEntries_Users_CommittedById",
                table: "ParameterProposalMappingEntries");

            migrationBuilder.RenameColumn(
                name: "CommittedOn",
                table: "ParameterProposalMappingEntries",
                newName: "ClosedOn");

            migrationBuilder.RenameColumn(
                name: "CommittedById",
                table: "ParameterProposalMappingEntries",
                newName: "ClosedById");

            migrationBuilder.RenameIndex(
                name: "IX_ParameterProposalMappingEntries_CommittedById",
                table: "ParameterProposalMappingEntries",
                newName: "IX_ParameterProposalMappingEntries_ClosedById");

            migrationBuilder.RenameColumn(
                name: "CommittedOn",
                table: "MethodProposalMappingEntries",
                newName: "ClosedOn");

            migrationBuilder.RenameColumn(
                name: "CommittedById",
                table: "MethodProposalMappingEntries",
                newName: "ClosedById");

            migrationBuilder.RenameIndex(
                name: "IX_MethodProposalMappingEntries_CommittedById",
                table: "MethodProposalMappingEntries",
                newName: "IX_MethodProposalMappingEntries_ClosedById");

            migrationBuilder.RenameColumn(
                name: "CommittedOn",
                table: "FieldProposalMappingEntries",
                newName: "ClosedOn");

            migrationBuilder.RenameColumn(
                name: "CommittedById",
                table: "FieldProposalMappingEntries",
                newName: "ClosedById");

            migrationBuilder.RenameIndex(
                name: "IX_FieldProposalMappingEntries_CommittedById",
                table: "FieldProposalMappingEntries",
                newName: "IX_FieldProposalMappingEntries_ClosedById");

            migrationBuilder.RenameColumn(
                name: "CommittedOn",
                table: "ClassProposalMappingEntries",
                newName: "ClosedOn");

            migrationBuilder.RenameColumn(
                name: "CommittedById",
                table: "ClassProposalMappingEntries",
                newName: "ClosedById");

            migrationBuilder.RenameIndex(
                name: "IX_ClassProposalMappingEntries_CommittedById",
                table: "ClassProposalMappingEntries",
                newName: "IX_ClassProposalMappingEntries_ClosedById");

            migrationBuilder.AddColumn<bool>(
                name: "Merged",
                table: "ParameterProposalMappingEntries",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Merged",
                table: "MethodProposalMappingEntries",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Merged",
                table: "FieldProposalMappingEntries",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Merged",
                table: "ClassProposalMappingEntries",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassProposalMappingEntries_Users_ClosedById",
                table: "ClassProposalMappingEntries",
                column: "ClosedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FieldProposalMappingEntries_Users_ClosedById",
                table: "FieldProposalMappingEntries",
                column: "ClosedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MethodProposalMappingEntries_Users_ClosedById",
                table: "MethodProposalMappingEntries",
                column: "ClosedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ParameterProposalMappingEntries_Users_ClosedById",
                table: "ParameterProposalMappingEntries",
                column: "ClosedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassProposalMappingEntries_Users_ClosedById",
                table: "ClassProposalMappingEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_FieldProposalMappingEntries_Users_ClosedById",
                table: "FieldProposalMappingEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_MethodProposalMappingEntries_Users_ClosedById",
                table: "MethodProposalMappingEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_ParameterProposalMappingEntries_Users_ClosedById",
                table: "ParameterProposalMappingEntries");

            migrationBuilder.DropColumn(
                name: "Merged",
                table: "ParameterProposalMappingEntries");

            migrationBuilder.DropColumn(
                name: "Merged",
                table: "MethodProposalMappingEntries");

            migrationBuilder.DropColumn(
                name: "Merged",
                table: "FieldProposalMappingEntries");

            migrationBuilder.DropColumn(
                name: "Merged",
                table: "ClassProposalMappingEntries");

            migrationBuilder.RenameColumn(
                name: "ClosedOn",
                table: "ParameterProposalMappingEntries",
                newName: "CommittedOn");

            migrationBuilder.RenameColumn(
                name: "ClosedById",
                table: "ParameterProposalMappingEntries",
                newName: "CommittedById");

            migrationBuilder.RenameIndex(
                name: "IX_ParameterProposalMappingEntries_ClosedById",
                table: "ParameterProposalMappingEntries",
                newName: "IX_ParameterProposalMappingEntries_CommittedById");

            migrationBuilder.RenameColumn(
                name: "ClosedOn",
                table: "MethodProposalMappingEntries",
                newName: "CommittedOn");

            migrationBuilder.RenameColumn(
                name: "ClosedById",
                table: "MethodProposalMappingEntries",
                newName: "CommittedById");

            migrationBuilder.RenameIndex(
                name: "IX_MethodProposalMappingEntries_ClosedById",
                table: "MethodProposalMappingEntries",
                newName: "IX_MethodProposalMappingEntries_CommittedById");

            migrationBuilder.RenameColumn(
                name: "ClosedOn",
                table: "FieldProposalMappingEntries",
                newName: "CommittedOn");

            migrationBuilder.RenameColumn(
                name: "ClosedById",
                table: "FieldProposalMappingEntries",
                newName: "CommittedById");

            migrationBuilder.RenameIndex(
                name: "IX_FieldProposalMappingEntries_ClosedById",
                table: "FieldProposalMappingEntries",
                newName: "IX_FieldProposalMappingEntries_CommittedById");

            migrationBuilder.RenameColumn(
                name: "ClosedOn",
                table: "ClassProposalMappingEntries",
                newName: "CommittedOn");

            migrationBuilder.RenameColumn(
                name: "ClosedById",
                table: "ClassProposalMappingEntries",
                newName: "CommittedById");

            migrationBuilder.RenameIndex(
                name: "IX_ClassProposalMappingEntries_ClosedById",
                table: "ClassProposalMappingEntries",
                newName: "IX_ClassProposalMappingEntries_CommittedById");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassProposalMappingEntries_Users_CommittedById",
                table: "ClassProposalMappingEntries",
                column: "CommittedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FieldProposalMappingEntries_Users_CommittedById",
                table: "FieldProposalMappingEntries",
                column: "CommittedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MethodProposalMappingEntries_Users_CommittedById",
                table: "MethodProposalMappingEntries",
                column: "CommittedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ParameterProposalMappingEntries_Users_CommittedById",
                table: "ParameterProposalMappingEntries",
                column: "CommittedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
