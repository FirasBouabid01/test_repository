using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTrainingLevelsSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrainingLevels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    SequenceOrder = table.Column<int>(type: "integer", nullable: false),
                    PrerequisiteLevelId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingLevels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingLevels_TrainingLevels_PrerequisiteLevelId",
                        column: x => x.PrerequisiteLevelId,
                        principalTable: "TrainingLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LeaderProgresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CurrentLevelId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaderProgresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeaderProgresses_TrainingLevels_CurrentLevelId",
                        column: x => x.CurrentLevelId,
                        principalTable: "TrainingLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LeaderProgresses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LevelHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    FromLevelId = table.Column<Guid>(type: "uuid", nullable: true),
                    ToLevelId = table.Column<Guid>(type: "uuid", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: false),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LevelHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LevelHistories_TrainingLevels_FromLevelId",
                        column: x => x.FromLevelId,
                        principalTable: "TrainingLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LevelHistories_TrainingLevels_ToLevelId",
                        column: x => x.ToLevelId,
                        principalTable: "TrainingLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LevelHistories_Users_ChangedBy",
                        column: x => x.ChangedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LevelHistories_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("d1111111-1111-1111-1111-111111111111"), "TrainingLevel.Read" },
                    { new Guid("d2222222-2222-2222-2222-222222222222"), "TrainingLevel.Create" },
                    { new Guid("d3333333-3333-3333-3333-333311111111"), "TrainingLevel.Update" },
                    { new Guid("d4444444-4444-4444-4444-444422222222"), "TrainingLevel.Delete" }
                });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { new Guid("d1111111-1111-1111-1111-111111111111"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("d2222222-2222-2222-2222-222222222222"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("d3333333-3333-3333-3333-333311111111"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("d4444444-4444-4444-4444-444422222222"), new Guid("11111111-1111-1111-1111-111111111111") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_LeaderProgresses_CurrentLevelId",
                table: "LeaderProgresses",
                column: "CurrentLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaderProgresses_UserId",
                table: "LeaderProgresses",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LevelHistories_ChangedBy",
                table: "LevelHistories",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_LevelHistories_FromLevelId",
                table: "LevelHistories",
                column: "FromLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_LevelHistories_ToLevelId",
                table: "LevelHistories",
                column: "ToLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_LevelHistories_UserId",
                table: "LevelHistories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingLevels_Name",
                table: "TrainingLevels",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrainingLevels_PrerequisiteLevelId",
                table: "TrainingLevels",
                column: "PrerequisiteLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingLevels_SequenceOrder",
                table: "TrainingLevels",
                column: "SequenceOrder");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LeaderProgresses");

            migrationBuilder.DropTable(
                name: "LevelHistories");

            migrationBuilder.DropTable(
                name: "TrainingLevels");

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("d1111111-1111-1111-1111-111111111111"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("d2222222-2222-2222-2222-222222222222"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("d3333333-3333-3333-3333-333311111111"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("d4444444-4444-4444-4444-444422222222"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d2222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d3333333-3333-3333-3333-333311111111"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d4444444-4444-4444-4444-444422222222"));
        }
    }
}
