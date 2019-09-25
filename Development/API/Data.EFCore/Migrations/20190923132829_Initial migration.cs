using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.EFCore.Migrations
{
    public partial class Initialmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Components",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Components", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameVersions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    IsPreRelease = table.Column<bool>(nullable: false),
                    IsSnapshot = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameVersions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MappingTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MappingTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VersionedComponents",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    GameVersionId = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ComponentId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VersionedComponents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VersionedComponents_Components_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "Components",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VersionedComponents_GameVersions_GameVersionId",
                        column: x => x.GameVersionId,
                        principalTable: "GameVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Releases",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    GameVersionId = table.Column<Guid>(nullable: false),
                    MappingTypeId = table.Column<Guid>(nullable: false),
                    IsSnapshot = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Releases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Releases_GameVersions_GameVersionId",
                        column: x => x.GameVersionId,
                        principalTable: "GameVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Releases_MappingTypes_MappingTypeId",
                        column: x => x.MappingTypeId,
                        principalTable: "MappingTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LiveMappingEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    VersionedComponentId = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    InputMapping = table.Column<string>(nullable: false),
                    OutputMapping = table.Column<string>(nullable: false),
                    Documentation = table.Column<string>(nullable: true),
                    Distribution = table.Column<int>(nullable: false),
                    MappingTypeId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiveMappingEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LiveMappingEntries_MappingTypes_MappingTypeId",
                        column: x => x.MappingTypeId,
                        principalTable: "MappingTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LiveMappingEntries_VersionedComponents_VersionedComponentId",
                        column: x => x.VersionedComponentId,
                        principalTable: "VersionedComponents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LockingEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    VersionedComponentId = table.Column<Guid>(nullable: false),
                    MappingTypeId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LockingEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LockingEntries_MappingTypes_MappingTypeId",
                        column: x => x.MappingTypeId,
                        principalTable: "MappingTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LockingEntries_VersionedComponents_VersionedComponentId",
                        column: x => x.VersionedComponentId,
                        principalTable: "VersionedComponents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VersionedComponentMetadata",
                columns: table => new
                {
                    VersionedComponentForeignKey = table.Column<Guid>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    OuterVersionedComponentForeignKey = table.Column<Guid>(nullable: true),
                    Package = table.Column<string>(nullable: true),
                    MemberOfVersionedComponentForeignKey = table.Column<Guid>(nullable: true),
                    IsStatic = table.Column<bool>(nullable: true),
                    MethodMetadata_MemberOfVersionedComponentForeignKey = table.Column<Guid>(nullable: true),
                    MethodMetadata_IsStatic = table.Column<bool>(nullable: true),
                    Descriptor = table.Column<string>(nullable: true),
                    ParameterOfVersionedComponentForeignKey = table.Column<Guid>(nullable: true),
                    Index = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VersionedComponentMetadata", x => x.VersionedComponentForeignKey);
                    table.ForeignKey(
                        name: "FK_VersionedComponentMetadata_VersionedComponentMetadata_Outer~",
                        column: x => x.OuterVersionedComponentForeignKey,
                        principalTable: "VersionedComponentMetadata",
                        principalColumn: "VersionedComponentForeignKey",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VersionedComponentMetadata_VersionedComponentMetadata_Membe~",
                        column: x => x.MemberOfVersionedComponentForeignKey,
                        principalTable: "VersionedComponentMetadata",
                        principalColumn: "VersionedComponentForeignKey",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VersionedComponentMetadata_VersionedComponentMetadata_Metho~",
                        column: x => x.MethodMetadata_MemberOfVersionedComponentForeignKey,
                        principalTable: "VersionedComponentMetadata",
                        principalColumn: "VersionedComponentForeignKey",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VersionedComponentMetadata_VersionedComponentMetadata_Param~",
                        column: x => x.ParameterOfVersionedComponentForeignKey,
                        principalTable: "VersionedComponentMetadata",
                        principalColumn: "VersionedComponentForeignKey",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VersionedComponentMetadata_VersionedComponents_VersionedCom~",
                        column: x => x.VersionedComponentForeignKey,
                        principalTable: "VersionedComponents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProposalMappingEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    VersionedComponentId = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    InputMapping = table.Column<string>(nullable: false),
                    OutputMapping = table.Column<string>(nullable: false),
                    Documentation = table.Column<string>(nullable: true),
                    Distribution = table.Column<int>(nullable: false),
                    MappingTypeId = table.Column<Guid>(nullable: false),
                    ProposedBy = table.Column<Guid>(nullable: false),
                    ProposedOn = table.Column<DateTime>(nullable: false),
                    IsOpen = table.Column<bool>(nullable: false),
                    IsPublicVote = table.Column<bool>(nullable: false),
                    VotedFor = table.Column<List<Guid>>(nullable: true),
                    VotedAgainst = table.Column<List<Guid>>(nullable: true),
                    Comment = table.Column<string>(nullable: false),
                    ClosedBy = table.Column<Guid>(nullable: true),
                    ClosedOn = table.Column<DateTime>(nullable: true),
                    Merged = table.Column<bool>(nullable: true),
                    WentLiveWithId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProposalMappingEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProposalMappingEntries_MappingTypes_MappingTypeId",
                        column: x => x.MappingTypeId,
                        principalTable: "MappingTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProposalMappingEntries_VersionedComponents_VersionedCompone~",
                        column: x => x.VersionedComponentId,
                        principalTable: "VersionedComponents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProposalMappingEntries_LiveMappingEntries_WentLiveWithId",
                        column: x => x.WentLiveWithId,
                        principalTable: "LiveMappingEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReleaseComponents",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ComponentType = table.Column<int>(nullable: false),
                    ReleaseId = table.Column<Guid>(nullable: false),
                    MemberId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReleaseComponents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReleaseComponents_LiveMappingEntries_MemberId",
                        column: x => x.MemberId,
                        principalTable: "LiveMappingEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReleaseComponents_Releases_ReleaseId",
                        column: x => x.ReleaseId,
                        principalTable: "Releases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameVersions_Name",
                table: "GameVersions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LiveMappingEntries_MappingTypeId",
                table: "LiveMappingEntries",
                column: "MappingTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveMappingEntries_VersionedComponentId",
                table: "LiveMappingEntries",
                column: "VersionedComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_LockingEntries_MappingTypeId",
                table: "LockingEntries",
                column: "MappingTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LockingEntries_VersionedComponentId",
                table: "LockingEntries",
                column: "VersionedComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProposalMappingEntries_MappingTypeId",
                table: "ProposalMappingEntries",
                column: "MappingTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProposalMappingEntries_VersionedComponentId",
                table: "ProposalMappingEntries",
                column: "VersionedComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProposalMappingEntries_WentLiveWithId",
                table: "ProposalMappingEntries",
                column: "WentLiveWithId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReleaseComponents_MemberId",
                table: "ReleaseComponents",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ReleaseComponents_ReleaseId",
                table: "ReleaseComponents",
                column: "ReleaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Releases_GameVersionId",
                table: "Releases",
                column: "GameVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_Releases_MappingTypeId",
                table: "Releases",
                column: "MappingTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Releases_Name",
                table: "Releases",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VersionedComponentMetadata_OuterVersionedComponentForeignKey",
                table: "VersionedComponentMetadata",
                column: "OuterVersionedComponentForeignKey");

            migrationBuilder.CreateIndex(
                name: "IX_VersionedComponentMetadata_MemberOfVersionedComponentForeig~",
                table: "VersionedComponentMetadata",
                column: "MemberOfVersionedComponentForeignKey");

            migrationBuilder.CreateIndex(
                name: "IX_VersionedComponentMetadata_MethodMetadata_MemberOfVersioned~",
                table: "VersionedComponentMetadata",
                column: "MethodMetadata_MemberOfVersionedComponentForeignKey");

            migrationBuilder.CreateIndex(
                name: "IX_VersionedComponentMetadata_ParameterOfVersionedComponentFor~",
                table: "VersionedComponentMetadata",
                column: "ParameterOfVersionedComponentForeignKey");

            migrationBuilder.CreateIndex(
                name: "IX_VersionedComponents_ComponentId",
                table: "VersionedComponents",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_VersionedComponents_GameVersionId",
                table: "VersionedComponents",
                column: "GameVersionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LockingEntries");

            migrationBuilder.DropTable(
                name: "ProposalMappingEntries");

            migrationBuilder.DropTable(
                name: "ReleaseComponents");

            migrationBuilder.DropTable(
                name: "VersionedComponentMetadata");

            migrationBuilder.DropTable(
                name: "LiveMappingEntries");

            migrationBuilder.DropTable(
                name: "Releases");

            migrationBuilder.DropTable(
                name: "VersionedComponents");

            migrationBuilder.DropTable(
                name: "MappingTypes");

            migrationBuilder.DropTable(
                name: "Components");

            migrationBuilder.DropTable(
                name: "GameVersions");
        }
    }
}
