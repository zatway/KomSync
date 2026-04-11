using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddHistoryDisplayNamesAndTaskHistoryFkSetNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectHistories_Users_ChangedById",
                table: "ProjectHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskHistories_Users_ChangedById",
                table: "TaskHistories");

            migrationBuilder.AlterColumn<Guid>(
                name: "ChangedById",
                table: "TaskHistories",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<string>(
                name: "ChangedByDisplayName",
                table: "TaskHistories",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChangedByDisplayName",
                table: "ProjectHistories",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectHistories_Users_ChangedById",
                table: "ProjectHistories",
                column: "ChangedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskHistories_Users_ChangedById",
                table: "TaskHistories",
                column: "ChangedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectHistories_Users_ChangedById",
                table: "ProjectHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskHistories_Users_ChangedById",
                table: "TaskHistories");

            migrationBuilder.DropColumn(
                name: "ChangedByDisplayName",
                table: "TaskHistories");

            migrationBuilder.DropColumn(
                name: "ChangedByDisplayName",
                table: "ProjectHistories");

            migrationBuilder.AlterColumn<Guid>(
                name: "ChangedById",
                table: "TaskHistories",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectHistories_Users_ChangedById",
                table: "ProjectHistories",
                column: "ChangedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskHistories_Users_ChangedById",
                table: "TaskHistories",
                column: "ChangedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
