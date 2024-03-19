using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChoreBoard.Data.Migrations
{
    public partial class AddCompletedBy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompletedById",
                schema: "app",
                table: "TaskInstance",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaskInstance_CompletedById",
                schema: "app",
                table: "TaskInstance",
                column: "CompletedById");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskInstance_CompletedById_FamilyMember",
                schema: "app",
                table: "TaskInstance",
                column: "CompletedById",
                principalSchema: "app",
                principalTable: "FamilyMember",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskInstance_CompletedById_FamilyMember",
                schema: "app",
                table: "TaskInstance");

            migrationBuilder.DropIndex(
                name: "IX_TaskInstance_CompletedById",
                schema: "app",
                table: "TaskInstance");

            migrationBuilder.DropColumn(
                name: "CompletedById",
                schema: "app",
                table: "TaskInstance");
        }
    }
}
