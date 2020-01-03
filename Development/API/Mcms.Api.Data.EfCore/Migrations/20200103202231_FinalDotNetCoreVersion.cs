using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.EFCore.Migrations
{
    public partial class FinalDotNetCoreVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProposalMappingEntries_LiveMappingEntries_WentLiveWithId",
                table: "ProposalMappingEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_ReleaseComponents_LiveMappingEntries_MemberId",
                table: "ReleaseComponents");

            migrationBuilder.DropForeignKey(
                name: "FK_VersionedComponentMetadata_VersionedComponentMetadata_Outer~",
                table: "VersionedComponentMetadata");

            migrationBuilder.DropForeignKey(
                name: "FK_VersionedComponentMetadata_VersionedComponents_VersionedCom~",
                table: "VersionedComponentMetadata");

            migrationBuilder.DropColumn(
                name: "ComponentType",
                table: "ReleaseComponents");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "ProposalMappingEntries");

            migrationBuilder.DropColumn(
                name: "ProposedOn",
                table: "ProposalMappingEntries");

            migrationBuilder.DropColumn(
                name: "VotedAgainst",
                table: "ProposalMappingEntries");

            migrationBuilder.DropColumn(
                name: "VotedFor",
                table: "ProposalMappingEntries");

            migrationBuilder.RenameColumn(
                name: "VersionedComponentForeignKey",
                table: "VersionedComponentMetadata",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ParameterOfVersionedComponentForeignKey",
                table: "VersionedComponentMetadata",
                newName: "ParameterOfId");

            migrationBuilder.RenameColumn(
                name: "MethodMetadata_MemberOfVersionedComponentForeignKey",
                table: "VersionedComponentMetadata",
                newName: "MethodMetadata_MemberOfId");

            migrationBuilder.RenameColumn(
                name: "MemberOfVersionedComponentForeignKey",
                table: "VersionedComponentMetadata",
                newName: "MemberOfId");

            migrationBuilder.RenameColumn(
                name: "Package",
                table: "VersionedComponentMetadata",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "OuterVersionedComponentForeignKey",
                table: "VersionedComponentMetadata",
                newName: "ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_VersionedComponentMetadata_ParameterOfVersionedComponentFor~",
                table: "VersionedComponentMetadata",
                newName: "IX_VersionedComponentMetadata_ParameterOfId");

            migrationBuilder.RenameIndex(
                name: "IX_VersionedComponentMetadata_MethodMetadata_MemberOfVersioned~",
                table: "VersionedComponentMetadata",
                newName: "IX_VersionedComponentMetadata_MethodMetadata_MemberOfId");

            migrationBuilder.RenameIndex(
                name: "IX_VersionedComponentMetadata_MemberOfVersionedComponentForeig~",
                table: "VersionedComponentMetadata",
                newName: "IX_VersionedComponentMetadata_MemberOfId");

            migrationBuilder.RenameIndex(
                name: "IX_VersionedComponentMetadata_OuterVersionedComponentForeignKey",
                table: "VersionedComponentMetadata",
                newName: "IX_VersionedComponentMetadata_ParentId");

            migrationBuilder.RenameColumn(
                name: "MemberId",
                table: "ReleaseComponents",
                newName: "MappingId");

            migrationBuilder.RenameIndex(
                name: "IX_ReleaseComponents_MemberId",
                table: "ReleaseComponents",
                newName: "IX_ReleaseComponents_MappingId");

            migrationBuilder.RenameColumn(
                name: "WentLiveWithId",
                table: "ProposalMappingEntries",
                newName: "CommittedWithId");

            migrationBuilder.RenameColumn(
                name: "ProposedBy",
                table: "ProposalMappingEntries",
                newName: "CreatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_ProposalMappingEntries_WentLiveWithId",
                table: "ProposalMappingEntries",
                newName: "IX_ProposalMappingEntries_CommittedWithId");

            migrationBuilder.AddColumn<Guid>(
                name: "OuterId",
                table: "VersionedComponentMetadata",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PackageId",
                table: "VersionedComponentMetadata",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "LiveMappingEntries",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Components",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Components",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "ClassInheritanceData",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SubclassId = table.Column<Guid>(nullable: true),
                    SuperclassId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassInheritanceData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassInheritanceData_VersionedComponentMetadata_SubclassId",
                        column: x => x.SubclassId,
                        principalTable: "VersionedComponentMetadata",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClassInheritanceData_VersionedComponentMetadata_SuperclassId",
                        column: x => x.SuperclassId,
                        principalTable: "VersionedComponentMetadata",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    Content = table.Column<string>(nullable: false),
                    HasBeenEdited = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedBy = table.Column<Guid>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    ProposedMappingId = table.Column<Guid>(nullable: true),
                    ReleaseId = table.Column<Guid>(nullable: true),
                    ParentId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comment_Comment_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Comment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comment_ProposalMappingEntries_ProposedMappingId",
                        column: x => x.ProposedMappingId,
                        principalTable: "ProposalMappingEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comment_Releases_ReleaseId",
                        column: x => x.ReleaseId,
                        principalTable: "Releases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VotingRecord",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProposalId = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    VotedBy = table.Column<Guid>(nullable: false),
                    IsForVote = table.Column<bool>(nullable: false),
                    HasBeenRescinded = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VotingRecord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VotingRecord_ProposalMappingEntries_ProposalId",
                        column: x => x.ProposalId,
                        principalTable: "ProposalMappingEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommentReaction",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    CommentId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentReaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommentReaction_Comment_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VersionedComponentMetadata_OuterId",
                table: "VersionedComponentMetadata",
                column: "OuterId");

            migrationBuilder.CreateIndex(
                name: "IX_VersionedComponentMetadata_PackageId",
                table: "VersionedComponentMetadata",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassInheritanceData_SubclassId",
                table: "ClassInheritanceData",
                column: "SubclassId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassInheritanceData_SuperclassId",
                table: "ClassInheritanceData",
                column: "SuperclassId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_ParentId",
                table: "Comment",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_ProposedMappingId",
                table: "Comment",
                column: "ProposedMappingId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_ReleaseId",
                table: "Comment",
                column: "ReleaseId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentReaction_CommentId",
                table: "CommentReaction",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_VotingRecord_ProposalId",
                table: "VotingRecord",
                column: "ProposalId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProposalMappingEntries_LiveMappingEntries_CommittedWithId",
                table: "ProposalMappingEntries",
                column: "CommittedWithId",
                principalTable: "LiveMappingEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReleaseComponents_LiveMappingEntries_MappingId",
                table: "ReleaseComponents",
                column: "MappingId",
                principalTable: "LiveMappingEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VersionedComponentMetadata_VersionedComponentMetadata_Outer~",
                table: "VersionedComponentMetadata",
                column: "OuterId",
                principalTable: "VersionedComponentMetadata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VersionedComponentMetadata_VersionedComponentMetadata_Packa~",
                table: "VersionedComponentMetadata",
                column: "PackageId",
                principalTable: "VersionedComponentMetadata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VersionedComponentMetadata_VersionedComponents_Id",
                table: "VersionedComponentMetadata",
                column: "Id",
                principalTable: "VersionedComponents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VersionedComponentMetadata_VersionedComponentMetadata_Paren~",
                table: "VersionedComponentMetadata",
                column: "ParentId",
                principalTable: "VersionedComponentMetadata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProposalMappingEntries_LiveMappingEntries_CommittedWithId",
                table: "ProposalMappingEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_ReleaseComponents_LiveMappingEntries_MappingId",
                table: "ReleaseComponents");

            migrationBuilder.DropForeignKey(
                name: "FK_VersionedComponentMetadata_VersionedComponentMetadata_Outer~",
                table: "VersionedComponentMetadata");

            migrationBuilder.DropForeignKey(
                name: "FK_VersionedComponentMetadata_VersionedComponentMetadata_Packa~",
                table: "VersionedComponentMetadata");

            migrationBuilder.DropForeignKey(
                name: "FK_VersionedComponentMetadata_VersionedComponents_Id",
                table: "VersionedComponentMetadata");

            migrationBuilder.DropForeignKey(
                name: "FK_VersionedComponentMetadata_VersionedComponentMetadata_Paren~",
                table: "VersionedComponentMetadata");

            migrationBuilder.DropTable(
                name: "ClassInheritanceData");

            migrationBuilder.DropTable(
                name: "CommentReaction");

            migrationBuilder.DropTable(
                name: "VotingRecord");

            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_VersionedComponentMetadata_OuterId",
                table: "VersionedComponentMetadata");

            migrationBuilder.DropIndex(
                name: "IX_VersionedComponentMetadata_PackageId",
                table: "VersionedComponentMetadata");

            migrationBuilder.DropColumn(
                name: "OuterId",
                table: "VersionedComponentMetadata");

            migrationBuilder.DropColumn(
                name: "PackageId",
                table: "VersionedComponentMetadata");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "LiveMappingEntries");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Components");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Components");

            migrationBuilder.RenameColumn(
                name: "ParameterOfId",
                table: "VersionedComponentMetadata",
                newName: "ParameterOfVersionedComponentForeignKey");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "VersionedComponentMetadata",
                newName: "OuterVersionedComponentForeignKey");

            migrationBuilder.RenameColumn(
                name: "MethodMetadata_MemberOfId",
                table: "VersionedComponentMetadata",
                newName: "MethodMetadata_MemberOfVersionedComponentForeignKey");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "VersionedComponentMetadata",
                newName: "VersionedComponentForeignKey");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "VersionedComponentMetadata",
                newName: "Package");

            migrationBuilder.RenameColumn(
                name: "MemberOfId",
                table: "VersionedComponentMetadata",
                newName: "MemberOfVersionedComponentForeignKey");

            migrationBuilder.RenameIndex(
                name: "IX_VersionedComponentMetadata_ParameterOfId",
                table: "VersionedComponentMetadata",
                newName: "IX_VersionedComponentMetadata_ParameterOfVersionedComponentFor~");

            migrationBuilder.RenameIndex(
                name: "IX_VersionedComponentMetadata_ParentId",
                table: "VersionedComponentMetadata",
                newName: "IX_VersionedComponentMetadata_OuterVersionedComponentForeignKey");

            migrationBuilder.RenameIndex(
                name: "IX_VersionedComponentMetadata_MethodMetadata_MemberOfId",
                table: "VersionedComponentMetadata",
                newName: "IX_VersionedComponentMetadata_MethodMetadata_MemberOfVersioned~");

            migrationBuilder.RenameIndex(
                name: "IX_VersionedComponentMetadata_MemberOfId",
                table: "VersionedComponentMetadata",
                newName: "IX_VersionedComponentMetadata_MemberOfVersionedComponentForeig~");

            migrationBuilder.RenameColumn(
                name: "MappingId",
                table: "ReleaseComponents",
                newName: "MemberId");

            migrationBuilder.RenameIndex(
                name: "IX_ReleaseComponents_MappingId",
                table: "ReleaseComponents",
                newName: "IX_ReleaseComponents_MemberId");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "ProposalMappingEntries",
                newName: "ProposedBy");

            migrationBuilder.RenameColumn(
                name: "CommittedWithId",
                table: "ProposalMappingEntries",
                newName: "WentLiveWithId");

            migrationBuilder.RenameIndex(
                name: "IX_ProposalMappingEntries_CommittedWithId",
                table: "ProposalMappingEntries",
                newName: "IX_ProposalMappingEntries_WentLiveWithId");

            migrationBuilder.AddColumn<int>(
                name: "ComponentType",
                table: "ReleaseComponents",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "ProposalMappingEntries",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ProposedOn",
                table: "ProposalMappingEntries",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<List<Guid>>(
                name: "VotedAgainst",
                table: "ProposalMappingEntries",
                nullable: true);

            migrationBuilder.AddColumn<List<Guid>>(
                name: "VotedFor",
                table: "ProposalMappingEntries",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProposalMappingEntries_LiveMappingEntries_WentLiveWithId",
                table: "ProposalMappingEntries",
                column: "WentLiveWithId",
                principalTable: "LiveMappingEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReleaseComponents_LiveMappingEntries_MemberId",
                table: "ReleaseComponents",
                column: "MemberId",
                principalTable: "LiveMappingEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VersionedComponentMetadata_VersionedComponentMetadata_Outer~",
                table: "VersionedComponentMetadata",
                column: "OuterVersionedComponentForeignKey",
                principalTable: "VersionedComponentMetadata",
                principalColumn: "VersionedComponentForeignKey",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VersionedComponentMetadata_VersionedComponents_VersionedCom~",
                table: "VersionedComponentMetadata",
                column: "VersionedComponentForeignKey",
                principalTable: "VersionedComponents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
