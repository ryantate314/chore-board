using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChoreBoard.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "app");

            migrationBuilder.EnsureSchema(
                name: "auth");

            migrationBuilder.CreateTable(
                name: "Family",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Uuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Family", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaskDefinition",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Uuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    ShortDescription = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Points = table.Column<int>(type: "int", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskDefinition", x => x.Id);
                    table.UniqueConstraint("AK_TaskDefinition_Uuid", x => x.Uuid);
                });

            migrationBuilder.CreateTable(
                name: "TaskStatus",
                schema: "app",
                columns: table => new
                {
                    StatusCode = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: false),
                    Description = table.Column<string>(type: "varchar(32)", unicode: false, maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskStatus", x => x.StatusCode);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "auth",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Uuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Email = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: false),
                    PasswordHash = table.Column<string>(type: "char(128)", unicode: false, fixedLength: true, maxLength: 128, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "FamilyMember",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Uuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    FirstName = table.Column<string>(type: "varchar(32)", unicode: false, maxLength: 32, nullable: false),
                    LastName = table.Column<string>(type: "varchar(32)", unicode: false, maxLength: 32, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    FamilyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamilyMember", x => x.Id);
                    table.UniqueConstraint("AK_FamilyMember_Uuid", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_FamilyMember_Family",
                        column: x => x.FamilyId,
                        principalSchema: "app",
                        principalTable: "Family",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskSchedule",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    RRule = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: false),
                    TaskDefinitionId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskSchedule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskSchedule_TaskDefinitionId_TaskDefinition",
                        column: x => x.TaskDefinitionId,
                        principalSchema: "app",
                        principalTable: "TaskDefinition",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TaskInstance",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Uuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    TaskDefinitionId = table.Column<int>(type: "int", nullable: false),
                    InstanceDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Status = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Points = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskInstance", x => x.Id);
                    table.UniqueConstraint("AK_TaskInstance_Uuid", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_TaskInstance_TaskDefinitionId_TaskDefinition",
                        column: x => x.TaskDefinitionId,
                        principalSchema: "app",
                        principalTable: "TaskDefinition",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaskInstance_TaskStatus",
                        column: x => x.Status,
                        principalSchema: "app",
                        principalTable: "TaskStatus",
                        principalColumn: "StatusCode");
                });

            migrationBuilder.InsertData(
                schema: "app",
                table: "TaskStatus",
                columns: new[] { "StatusCode", "Description" },
                values: new object[,]
                {
                    { "C", "Completed" },
                    { "D", "Deleted" },
                    { "I", "In Progress" },
                    { "T", "To Do" },
                    { "U", "Upcoming" }
                });

            migrationBuilder.CreateIndex(
                name: "UQ_Family_Uuid",
                schema: "app",
                table: "Family",
                column: "Uuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FamilyMember_FamilyId",
                schema: "app",
                table: "FamilyMember",
                column: "FamilyId");

            migrationBuilder.CreateIndex(
                name: "UQ_FamilyMember_Uuid",
                schema: "app",
                table: "FamilyMember",
                column: "Uuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_TaskDefinition_Uuid",
                schema: "app",
                table: "TaskDefinition",
                column: "Uuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaskInstance_Status",
                schema: "app",
                table: "TaskInstance",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_TaskInstance_TaskDefinitionId",
                schema: "app",
                table: "TaskInstance",
                column: "TaskDefinitionId");

            migrationBuilder.CreateIndex(
                name: "UQ_TaskInstance_Uuid",
                schema: "app",
                table: "TaskInstance",
                column: "Uuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaskSchedule_TaskDefinitionId",
                schema: "app",
                table: "TaskSchedule",
                column: "TaskDefinitionId");

            migrationBuilder.CreateIndex(
                name: "UQ_User_Uuid",
                schema: "auth",
                table: "User",
                column: "Uuid",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FamilyMember",
                schema: "app");

            migrationBuilder.DropTable(
                name: "TaskInstance",
                schema: "app");

            migrationBuilder.DropTable(
                name: "TaskSchedule",
                schema: "app");

            migrationBuilder.DropTable(
                name: "User",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "Family",
                schema: "app");

            migrationBuilder.DropTable(
                name: "TaskStatus",
                schema: "app");

            migrationBuilder.DropTable(
                name: "TaskDefinition",
                schema: "app");
        }
    }
}
