using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tasks.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "List",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Title = table.Column<string>(maxLength: 255, nullable: false),
                    LastUpdated = table.Column<DateTime>(nullable: false),
                    Tenant = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_List", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Tenant = table.Column<string>(maxLength: 64, nullable: false),
                    Title = table.Column<string>(maxLength: 500, nullable: false),
                    Updated = table.Column<DateTime>(nullable: false),
                    Position = table.Column<string>(maxLength: 20, nullable: false),
                    Notes = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: false),
                    Due = table.Column<DateTime>(nullable: true),
                    Completed = table.Column<DateTime>(nullable: true),
                    ParentTaskListId = table.Column<string>(nullable: true),
                    ParentId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_Tasks_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tasks_List_ParentTaskListId",
                        column: x => x.ParentTaskListId,
                        principalTable: "List",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ParentId",
                table: "Tasks",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ParentTaskListId",
                table: "Tasks",
                column: "ParentTaskListId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "List");
        }
    }
}
