using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddKnowledgeArticleProjectScope : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "KnowledgeArticles",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectTaskId",
                table: "KnowledgeArticles",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_KnowledgeArticles_ProjectId",
                table: "KnowledgeArticles",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_KnowledgeArticles_ProjectTaskId",
                table: "KnowledgeArticles",
                column: "ProjectTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_KnowledgeArticles_Projects_ProjectId",
                table: "KnowledgeArticles",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_KnowledgeArticles_Tasks_ProjectTaskId",
                table: "KnowledgeArticles",
                column: "ProjectTaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KnowledgeArticles_Projects_ProjectId",
                table: "KnowledgeArticles");

            migrationBuilder.DropForeignKey(
                name: "FK_KnowledgeArticles_Tasks_ProjectTaskId",
                table: "KnowledgeArticles");

            migrationBuilder.DropIndex(
                name: "IX_KnowledgeArticles_ProjectId",
                table: "KnowledgeArticles");

            migrationBuilder.DropIndex(
                name: "IX_KnowledgeArticles_ProjectTaskId",
                table: "KnowledgeArticles");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "KnowledgeArticles");

            migrationBuilder.DropColumn(
                name: "ProjectTaskId",
                table: "KnowledgeArticles");
        }
    }
}
