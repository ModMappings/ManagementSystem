using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.EFCore.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClassMappings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassMappings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FieldMappings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldMappings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MethodMappings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MethodMappings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ParameterMappings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParameterMappings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClassReleaseMember",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ReleaseId = table.Column<Guid>(nullable: false),
                    MemberId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassReleaseMember", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FieldVersionedMappings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    GameVersionId = table.Column<Guid>(nullable: false),
                    CreatedById = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    MappingId = table.Column<Guid>(nullable: false),
                    MemberOfId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldVersionedMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FieldVersionedMappings_FieldMappings_MappingId",
                        column: x => x.MappingId,
                        principalTable: "FieldMappings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MethodVersionedMappings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    GameVersionId = table.Column<Guid>(nullable: false),
                    CreatedById = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    MappingId = table.Column<Guid>(nullable: false),
                    MemberOfId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MethodVersionedMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MethodVersionedMappings_MethodMappings_MappingId",
                        column: x => x.MappingId,
                        principalTable: "MethodMappings",
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
                    CreatedById = table.Column<Guid>(nullable: false),
                    GameVersionId = table.Column<Guid>(nullable: false),
                    ClassCommittedMappingEntryId = table.Column<Guid>(nullable: true),
                    FieldCommittedMappingEntryId = table.Column<Guid>(nullable: true),
                    MethodCommittedMappingEntryId = table.Column<Guid>(nullable: true),
                    ParameterCommittedMappingEntryId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Releases", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClassVersionedMappings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    GameVersionId = table.Column<Guid>(nullable: false),
                    CreatedById = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    MappingId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassVersionedMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassVersionedMappings_ClassMappings_MappingId",
                        column: x => x.MappingId,
                        principalTable: "ClassMappings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClassCommittedMappingEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    VersionedMappingId = table.Column<Guid>(nullable: false),
                    InputMappingType = table.Column<string>(nullable: false),
                    InputMapping = table.Column<string>(nullable: false),
                    OutputMappingType = table.Column<string>(nullable: false),
                    OutputMapping = table.Column<string>(nullable: false),
                    ProposalId = table.Column<Guid>(nullable: false),
                    Package = table.Column<string>(nullable: false),
                    ParentId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassCommittedMappingEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassCommittedMappingEntries_ClassCommittedMappingEntries_P~",
                        column: x => x.ParentId,
                        principalTable: "ClassCommittedMappingEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClassCommittedMappingEntries_ClassVersionedMappings_Version~",
                        column: x => x.VersionedMappingId,
                        principalTable: "ClassVersionedMappings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    CanEdit = table.Column<bool>(nullable: false),
                    CanReview = table.Column<bool>(nullable: false),
                    CanCommit = table.Column<bool>(nullable: false),
                    CanRelease = table.Column<bool>(nullable: false),
                    CanCreateGameVersions = table.Column<bool>(nullable: false),
                    ClassProposalMappingEntryId = table.Column<Guid>(nullable: true),
                    ClassProposalMappingEntryId1 = table.Column<Guid>(nullable: true),
                    FieldProposalMappingEntryId = table.Column<Guid>(nullable: true),
                    FieldProposalMappingEntryId1 = table.Column<Guid>(nullable: true),
                    MethodProposalMappingEntryId = table.Column<Guid>(nullable: true),
                    MethodProposalMappingEntryId1 = table.Column<Guid>(nullable: true),
                    ParameterProposalMappingEntryId = table.Column<Guid>(nullable: true),
                    ParameterProposalMappingEntryId1 = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClassProposalMappingEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    VersionedMappingId = table.Column<Guid>(nullable: false),
                    InputMappingType = table.Column<string>(nullable: false),
                    InputMapping = table.Column<string>(nullable: false),
                    OutputMappingType = table.Column<string>(nullable: false),
                    OutputMapping = table.Column<string>(nullable: false),
                    ProposedById = table.Column<Guid>(nullable: false),
                    ProposedOn = table.Column<DateTime>(nullable: false),
                    IsPublicVote = table.Column<bool>(nullable: false),
                    Comment = table.Column<string>(nullable: false),
                    CommittedById = table.Column<Guid>(nullable: true),
                    CommittedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassProposalMappingEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassProposalMappingEntries_Users_CommittedById",
                        column: x => x.CommittedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClassProposalMappingEntries_Users_ProposedById",
                        column: x => x.ProposedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassProposalMappingEntries_ClassVersionedMappings_Versione~",
                        column: x => x.VersionedMappingId,
                        principalTable: "ClassVersionedMappings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FieldProposalMappingEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    VersionedMappingId = table.Column<Guid>(nullable: false),
                    InputMappingType = table.Column<string>(nullable: false),
                    InputMapping = table.Column<string>(nullable: false),
                    OutputMappingType = table.Column<string>(nullable: false),
                    OutputMapping = table.Column<string>(nullable: false),
                    ProposedById = table.Column<Guid>(nullable: false),
                    ProposedOn = table.Column<DateTime>(nullable: false),
                    IsPublicVote = table.Column<bool>(nullable: false),
                    Comment = table.Column<string>(nullable: false),
                    CommittedById = table.Column<Guid>(nullable: true),
                    CommittedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldProposalMappingEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FieldProposalMappingEntries_Users_CommittedById",
                        column: x => x.CommittedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FieldProposalMappingEntries_Users_ProposedById",
                        column: x => x.ProposedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FieldProposalMappingEntries_FieldVersionedMappings_Versione~",
                        column: x => x.VersionedMappingId,
                        principalTable: "FieldVersionedMappings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameVersions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<Guid>(nullable: false),
                    IsPreRelease = table.Column<bool>(nullable: false),
                    IsSnapshot = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameVersions_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MethodProposalMappingEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    VersionedMappingId = table.Column<Guid>(nullable: false),
                    InputMappingType = table.Column<string>(nullable: false),
                    InputMapping = table.Column<string>(nullable: false),
                    OutputMappingType = table.Column<string>(nullable: false),
                    OutputMapping = table.Column<string>(nullable: false),
                    ProposedById = table.Column<Guid>(nullable: false),
                    ProposedOn = table.Column<DateTime>(nullable: false),
                    IsPublicVote = table.Column<bool>(nullable: false),
                    Comment = table.Column<string>(nullable: false),
                    CommittedById = table.Column<Guid>(nullable: true),
                    CommittedOn = table.Column<DateTime>(nullable: false),
                    IsStatic = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MethodProposalMappingEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MethodProposalMappingEntries_Users_CommittedById",
                        column: x => x.CommittedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MethodProposalMappingEntries_Users_ProposedById",
                        column: x => x.ProposedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MethodProposalMappingEntries_MethodVersionedMappings_Versio~",
                        column: x => x.VersionedMappingId,
                        principalTable: "MethodVersionedMappings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FieldCommittedMappingEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    VersionedMappingId = table.Column<Guid>(nullable: false),
                    InputMappingType = table.Column<string>(nullable: false),
                    InputMapping = table.Column<string>(nullable: false),
                    OutputMappingType = table.Column<string>(nullable: false),
                    OutputMapping = table.Column<string>(nullable: false),
                    ProposalId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldCommittedMappingEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FieldCommittedMappingEntries_FieldProposalMappingEntries_Pr~",
                        column: x => x.ProposalId,
                        principalTable: "FieldProposalMappingEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FieldCommittedMappingEntries_FieldVersionedMappings_Version~",
                        column: x => x.VersionedMappingId,
                        principalTable: "FieldVersionedMappings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MethodCommittedMappingEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    VersionedMappingId = table.Column<Guid>(nullable: false),
                    InputMappingType = table.Column<string>(nullable: false),
                    InputMapping = table.Column<string>(nullable: false),
                    OutputMappingType = table.Column<string>(nullable: false),
                    OutputMapping = table.Column<string>(nullable: false),
                    ProposalId = table.Column<Guid>(nullable: false),
                    IsStatic = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MethodCommittedMappingEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MethodCommittedMappingEntries_MethodProposalMappingEntries_~",
                        column: x => x.ProposalId,
                        principalTable: "MethodProposalMappingEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MethodCommittedMappingEntries_MethodVersionedMappings_Versi~",
                        column: x => x.VersionedMappingId,
                        principalTable: "MethodVersionedMappings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FieldReleaseMember",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ReleaseId = table.Column<Guid>(nullable: false),
                    MemberId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldReleaseMember", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FieldReleaseMember_FieldCommittedMappingEntries_MemberId",
                        column: x => x.MemberId,
                        principalTable: "FieldCommittedMappingEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FieldReleaseMember_Releases_ReleaseId",
                        column: x => x.ReleaseId,
                        principalTable: "Releases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MethodReleaseMember",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ReleaseId = table.Column<Guid>(nullable: false),
                    MemberId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MethodReleaseMember", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MethodReleaseMember_MethodCommittedMappingEntries_MemberId",
                        column: x => x.MemberId,
                        principalTable: "MethodCommittedMappingEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MethodReleaseMember_Releases_ReleaseId",
                        column: x => x.ReleaseId,
                        principalTable: "Releases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParameterVersionedMappings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    GameVersionId = table.Column<Guid>(nullable: false),
                    CreatedById = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    MappingId = table.Column<Guid>(nullable: false),
                    ParameterOfId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParameterVersionedMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParameterVersionedMappings_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParameterVersionedMappings_GameVersions_GameVersionId",
                        column: x => x.GameVersionId,
                        principalTable: "GameVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParameterVersionedMappings_ParameterMappings_MappingId",
                        column: x => x.MappingId,
                        principalTable: "ParameterMappings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParameterVersionedMappings_MethodCommittedMappingEntries_Pa~",
                        column: x => x.ParameterOfId,
                        principalTable: "MethodCommittedMappingEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ParameterProposalMappingEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    VersionedMappingId = table.Column<Guid>(nullable: false),
                    InputMappingType = table.Column<string>(nullable: false),
                    InputMapping = table.Column<string>(nullable: false),
                    OutputMappingType = table.Column<string>(nullable: false),
                    OutputMapping = table.Column<string>(nullable: false),
                    ProposedById = table.Column<Guid>(nullable: false),
                    ProposedOn = table.Column<DateTime>(nullable: false),
                    IsPublicVote = table.Column<bool>(nullable: false),
                    Comment = table.Column<string>(nullable: false),
                    CommittedById = table.Column<Guid>(nullable: true),
                    CommittedOn = table.Column<DateTime>(nullable: false),
                    Index = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParameterProposalMappingEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParameterProposalMappingEntries_Users_CommittedById",
                        column: x => x.CommittedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ParameterProposalMappingEntries_Users_ProposedById",
                        column: x => x.ProposedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParameterProposalMappingEntries_ParameterVersionedMappings_~",
                        column: x => x.VersionedMappingId,
                        principalTable: "ParameterVersionedMappings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParameterCommittedMappingEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    VersionedMappingId = table.Column<Guid>(nullable: false),
                    InputMappingType = table.Column<string>(nullable: false),
                    InputMapping = table.Column<string>(nullable: false),
                    OutputMappingType = table.Column<string>(nullable: false),
                    OutputMapping = table.Column<string>(nullable: false),
                    ProposalId = table.Column<Guid>(nullable: false),
                    Index = table.Column<int>(nullable: false),
                    MethodVersionedMappingId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParameterCommittedMappingEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParameterCommittedMappingEntries_MethodVersionedMappings_Me~",
                        column: x => x.MethodVersionedMappingId,
                        principalTable: "MethodVersionedMappings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ParameterCommittedMappingEntries_ParameterProposalMappingEn~",
                        column: x => x.ProposalId,
                        principalTable: "ParameterProposalMappingEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParameterCommittedMappingEntries_ParameterVersionedMappings~",
                        column: x => x.VersionedMappingId,
                        principalTable: "ParameterVersionedMappings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParameterReleaseMember",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ReleaseId = table.Column<Guid>(nullable: false),
                    MemberId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParameterReleaseMember", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParameterReleaseMember_ParameterCommittedMappingEntries_Mem~",
                        column: x => x.MemberId,
                        principalTable: "ParameterCommittedMappingEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParameterReleaseMember_Releases_ReleaseId",
                        column: x => x.ReleaseId,
                        principalTable: "Releases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClassCommittedMappingEntries_ParentId",
                table: "ClassCommittedMappingEntries",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassCommittedMappingEntries_ProposalId",
                table: "ClassCommittedMappingEntries",
                column: "ProposalId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassCommittedMappingEntries_VersionedMappingId",
                table: "ClassCommittedMappingEntries",
                column: "VersionedMappingId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassProposalMappingEntries_CommittedById",
                table: "ClassProposalMappingEntries",
                column: "CommittedById");

            migrationBuilder.CreateIndex(
                name: "IX_ClassProposalMappingEntries_ProposedById",
                table: "ClassProposalMappingEntries",
                column: "ProposedById");

            migrationBuilder.CreateIndex(
                name: "IX_ClassProposalMappingEntries_VersionedMappingId",
                table: "ClassProposalMappingEntries",
                column: "VersionedMappingId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassReleaseMember_MemberId",
                table: "ClassReleaseMember",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassReleaseMember_ReleaseId",
                table: "ClassReleaseMember",
                column: "ReleaseId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassVersionedMappings_CreatedById",
                table: "ClassVersionedMappings",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ClassVersionedMappings_GameVersionId",
                table: "ClassVersionedMappings",
                column: "GameVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassVersionedMappings_MappingId",
                table: "ClassVersionedMappings",
                column: "MappingId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldCommittedMappingEntries_ProposalId",
                table: "FieldCommittedMappingEntries",
                column: "ProposalId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldCommittedMappingEntries_VersionedMappingId",
                table: "FieldCommittedMappingEntries",
                column: "VersionedMappingId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldProposalMappingEntries_CommittedById",
                table: "FieldProposalMappingEntries",
                column: "CommittedById");

            migrationBuilder.CreateIndex(
                name: "IX_FieldProposalMappingEntries_ProposedById",
                table: "FieldProposalMappingEntries",
                column: "ProposedById");

            migrationBuilder.CreateIndex(
                name: "IX_FieldProposalMappingEntries_VersionedMappingId",
                table: "FieldProposalMappingEntries",
                column: "VersionedMappingId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldReleaseMember_MemberId",
                table: "FieldReleaseMember",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldReleaseMember_ReleaseId",
                table: "FieldReleaseMember",
                column: "ReleaseId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldVersionedMappings_CreatedById",
                table: "FieldVersionedMappings",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_FieldVersionedMappings_GameVersionId",
                table: "FieldVersionedMappings",
                column: "GameVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldVersionedMappings_MappingId",
                table: "FieldVersionedMappings",
                column: "MappingId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldVersionedMappings_MemberOfId",
                table: "FieldVersionedMappings",
                column: "MemberOfId");

            migrationBuilder.CreateIndex(
                name: "IX_GameVersions_CreatedById",
                table: "GameVersions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_MethodCommittedMappingEntries_ProposalId",
                table: "MethodCommittedMappingEntries",
                column: "ProposalId");

            migrationBuilder.CreateIndex(
                name: "IX_MethodCommittedMappingEntries_VersionedMappingId",
                table: "MethodCommittedMappingEntries",
                column: "VersionedMappingId");

            migrationBuilder.CreateIndex(
                name: "IX_MethodProposalMappingEntries_CommittedById",
                table: "MethodProposalMappingEntries",
                column: "CommittedById");

            migrationBuilder.CreateIndex(
                name: "IX_MethodProposalMappingEntries_ProposedById",
                table: "MethodProposalMappingEntries",
                column: "ProposedById");

            migrationBuilder.CreateIndex(
                name: "IX_MethodProposalMappingEntries_VersionedMappingId",
                table: "MethodProposalMappingEntries",
                column: "VersionedMappingId");

            migrationBuilder.CreateIndex(
                name: "IX_MethodReleaseMember_MemberId",
                table: "MethodReleaseMember",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MethodReleaseMember_ReleaseId",
                table: "MethodReleaseMember",
                column: "ReleaseId");

            migrationBuilder.CreateIndex(
                name: "IX_MethodVersionedMappings_CreatedById",
                table: "MethodVersionedMappings",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_MethodVersionedMappings_GameVersionId",
                table: "MethodVersionedMappings",
                column: "GameVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_MethodVersionedMappings_MappingId",
                table: "MethodVersionedMappings",
                column: "MappingId");

            migrationBuilder.CreateIndex(
                name: "IX_MethodVersionedMappings_MemberOfId",
                table: "MethodVersionedMappings",
                column: "MemberOfId");

            migrationBuilder.CreateIndex(
                name: "IX_ParameterCommittedMappingEntries_MethodVersionedMappingId",
                table: "ParameterCommittedMappingEntries",
                column: "MethodVersionedMappingId");

            migrationBuilder.CreateIndex(
                name: "IX_ParameterCommittedMappingEntries_ProposalId",
                table: "ParameterCommittedMappingEntries",
                column: "ProposalId");

            migrationBuilder.CreateIndex(
                name: "IX_ParameterCommittedMappingEntries_VersionedMappingId",
                table: "ParameterCommittedMappingEntries",
                column: "VersionedMappingId");

            migrationBuilder.CreateIndex(
                name: "IX_ParameterProposalMappingEntries_CommittedById",
                table: "ParameterProposalMappingEntries",
                column: "CommittedById");

            migrationBuilder.CreateIndex(
                name: "IX_ParameterProposalMappingEntries_ProposedById",
                table: "ParameterProposalMappingEntries",
                column: "ProposedById");

            migrationBuilder.CreateIndex(
                name: "IX_ParameterProposalMappingEntries_VersionedMappingId",
                table: "ParameterProposalMappingEntries",
                column: "VersionedMappingId");

            migrationBuilder.CreateIndex(
                name: "IX_ParameterReleaseMember_MemberId",
                table: "ParameterReleaseMember",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ParameterReleaseMember_ReleaseId",
                table: "ParameterReleaseMember",
                column: "ReleaseId");

            migrationBuilder.CreateIndex(
                name: "IX_ParameterVersionedMappings_CreatedById",
                table: "ParameterVersionedMappings",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ParameterVersionedMappings_GameVersionId",
                table: "ParameterVersionedMappings",
                column: "GameVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_ParameterVersionedMappings_MappingId",
                table: "ParameterVersionedMappings",
                column: "MappingId");

            migrationBuilder.CreateIndex(
                name: "IX_ParameterVersionedMappings_ParameterOfId",
                table: "ParameterVersionedMappings",
                column: "ParameterOfId");

            migrationBuilder.CreateIndex(
                name: "IX_Releases_ClassCommittedMappingEntryId",
                table: "Releases",
                column: "ClassCommittedMappingEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_Releases_CreatedById",
                table: "Releases",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Releases_FieldCommittedMappingEntryId",
                table: "Releases",
                column: "FieldCommittedMappingEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_Releases_GameVersionId",
                table: "Releases",
                column: "GameVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_Releases_MethodCommittedMappingEntryId",
                table: "Releases",
                column: "MethodCommittedMappingEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_Releases_ParameterCommittedMappingEntryId",
                table: "Releases",
                column: "ParameterCommittedMappingEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ClassProposalMappingEntryId",
                table: "Users",
                column: "ClassProposalMappingEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ClassProposalMappingEntryId1",
                table: "Users",
                column: "ClassProposalMappingEntryId1");

            migrationBuilder.CreateIndex(
                name: "IX_Users_FieldProposalMappingEntryId",
                table: "Users",
                column: "FieldProposalMappingEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_FieldProposalMappingEntryId1",
                table: "Users",
                column: "FieldProposalMappingEntryId1");

            migrationBuilder.CreateIndex(
                name: "IX_Users_MethodProposalMappingEntryId",
                table: "Users",
                column: "MethodProposalMappingEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_MethodProposalMappingEntryId1",
                table: "Users",
                column: "MethodProposalMappingEntryId1");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ParameterProposalMappingEntryId",
                table: "Users",
                column: "ParameterProposalMappingEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ParameterProposalMappingEntryId1",
                table: "Users",
                column: "ParameterProposalMappingEntryId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassReleaseMember_ClassCommittedMappingEntries_MemberId",
                table: "ClassReleaseMember",
                column: "MemberId",
                principalTable: "ClassCommittedMappingEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassReleaseMember_Releases_ReleaseId",
                table: "ClassReleaseMember",
                column: "ReleaseId",
                principalTable: "Releases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FieldVersionedMappings_Users_CreatedById",
                table: "FieldVersionedMappings",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FieldVersionedMappings_ClassCommittedMappingEntries_MemberO~",
                table: "FieldVersionedMappings",
                column: "MemberOfId",
                principalTable: "ClassCommittedMappingEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FieldVersionedMappings_GameVersions_GameVersionId",
                table: "FieldVersionedMappings",
                column: "GameVersionId",
                principalTable: "GameVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MethodVersionedMappings_Users_CreatedById",
                table: "MethodVersionedMappings",
                column: "CreatedById",
                principalTable: "Users",
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
                name: "FK_MethodVersionedMappings_GameVersions_GameVersionId",
                table: "MethodVersionedMappings",
                column: "GameVersionId",
                principalTable: "GameVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Releases_Users_CreatedById",
                table: "Releases",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Releases_ClassCommittedMappingEntries_ClassCommittedMapping~",
                table: "Releases",
                column: "ClassCommittedMappingEntryId",
                principalTable: "ClassCommittedMappingEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Releases_GameVersions_GameVersionId",
                table: "Releases",
                column: "GameVersionId",
                principalTable: "GameVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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

            migrationBuilder.AddForeignKey(
                name: "FK_ClassVersionedMappings_Users_CreatedById",
                table: "ClassVersionedMappings",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassVersionedMappings_GameVersions_GameVersionId",
                table: "ClassVersionedMappings",
                column: "GameVersionId",
                principalTable: "GameVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassCommittedMappingEntries_ClassProposalMappingEntries_Pr~",
                table: "ClassCommittedMappingEntries",
                column: "ProposalId",
                principalTable: "ClassProposalMappingEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_ClassProposalMappingEntries_ClassProposalMappingEntry~",
                table: "Users",
                column: "ClassProposalMappingEntryId",
                principalTable: "ClassProposalMappingEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_ClassProposalMappingEntries_ClassProposalMappingEntr~1",
                table: "Users",
                column: "ClassProposalMappingEntryId1",
                principalTable: "ClassProposalMappingEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_FieldProposalMappingEntries_FieldProposalMappingEntry~",
                table: "Users",
                column: "FieldProposalMappingEntryId",
                principalTable: "FieldProposalMappingEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_FieldProposalMappingEntries_FieldProposalMappingEntr~1",
                table: "Users",
                column: "FieldProposalMappingEntryId1",
                principalTable: "FieldProposalMappingEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_MethodProposalMappingEntries_MethodProposalMappingEnt~",
                table: "Users",
                column: "MethodProposalMappingEntryId",
                principalTable: "MethodProposalMappingEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_MethodProposalMappingEntries_MethodProposalMappingEn~1",
                table: "Users",
                column: "MethodProposalMappingEntryId1",
                principalTable: "MethodProposalMappingEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_ParameterProposalMappingEntries_ParameterProposalMapp~",
                table: "Users",
                column: "ParameterProposalMappingEntryId",
                principalTable: "ParameterProposalMappingEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_ParameterProposalMappingEntries_ParameterProposalMap~1",
                table: "Users",
                column: "ParameterProposalMappingEntryId1",
                principalTable: "ParameterProposalMappingEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassCommittedMappingEntries_ClassProposalMappingEntries_Pr~",
                table: "ClassCommittedMappingEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_ClassProposalMappingEntries_ClassProposalMappingEntry~",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_ClassProposalMappingEntries_ClassProposalMappingEntr~1",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassCommittedMappingEntries_ClassVersionedMappings_Version~",
                table: "ClassCommittedMappingEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_FieldProposalMappingEntries_Users_CommittedById",
                table: "FieldProposalMappingEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_FieldProposalMappingEntries_Users_ProposedById",
                table: "FieldProposalMappingEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_FieldVersionedMappings_Users_CreatedById",
                table: "FieldVersionedMappings");

            migrationBuilder.DropForeignKey(
                name: "FK_GameVersions_Users_CreatedById",
                table: "GameVersions");

            migrationBuilder.DropForeignKey(
                name: "FK_MethodProposalMappingEntries_Users_CommittedById",
                table: "MethodProposalMappingEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_MethodProposalMappingEntries_Users_ProposedById",
                table: "MethodProposalMappingEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_MethodVersionedMappings_Users_CreatedById",
                table: "MethodVersionedMappings");

            migrationBuilder.DropForeignKey(
                name: "FK_ParameterProposalMappingEntries_Users_CommittedById",
                table: "ParameterProposalMappingEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_ParameterProposalMappingEntries_Users_ProposedById",
                table: "ParameterProposalMappingEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_ParameterVersionedMappings_Users_CreatedById",
                table: "ParameterVersionedMappings");

            migrationBuilder.DropTable(
                name: "ClassReleaseMember");

            migrationBuilder.DropTable(
                name: "FieldReleaseMember");

            migrationBuilder.DropTable(
                name: "MethodReleaseMember");

            migrationBuilder.DropTable(
                name: "ParameterReleaseMember");

            migrationBuilder.DropTable(
                name: "Releases");

            migrationBuilder.DropTable(
                name: "FieldCommittedMappingEntries");

            migrationBuilder.DropTable(
                name: "ParameterCommittedMappingEntries");

            migrationBuilder.DropTable(
                name: "ClassProposalMappingEntries");

            migrationBuilder.DropTable(
                name: "ClassVersionedMappings");

            migrationBuilder.DropTable(
                name: "ClassMappings");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "FieldProposalMappingEntries");

            migrationBuilder.DropTable(
                name: "ParameterProposalMappingEntries");

            migrationBuilder.DropTable(
                name: "FieldVersionedMappings");

            migrationBuilder.DropTable(
                name: "ParameterVersionedMappings");

            migrationBuilder.DropTable(
                name: "FieldMappings");

            migrationBuilder.DropTable(
                name: "ParameterMappings");

            migrationBuilder.DropTable(
                name: "MethodCommittedMappingEntries");

            migrationBuilder.DropTable(
                name: "MethodProposalMappingEntries");

            migrationBuilder.DropTable(
                name: "MethodVersionedMappings");

            migrationBuilder.DropTable(
                name: "GameVersions");

            migrationBuilder.DropTable(
                name: "MethodMappings");

            migrationBuilder.DropTable(
                name: "ClassCommittedMappingEntries");
        }
    }
}
