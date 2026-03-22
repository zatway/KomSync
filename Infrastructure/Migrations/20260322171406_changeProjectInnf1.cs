using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changeProjectInnf1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectHistories_Users_ChangedById",
                table: "ProjectHistories");

            migrationBuilder.AlterColumn<Guid>(
                name: "ChangedById",
                table: "ProjectHistories",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Avatar", "CreatedAt", "DepartmentId", "Email", "FullName", "NormalizedEmail", "PasswordHash", "PositionId", "Role", "UpdatedAt" },
                values: new object[] { new Guid("10ca3dbd-eaa5-4361-9a0f-49be6bc8549d"), null, new DateTimeOffset(new DateTime(2026, 3, 22, 17, 14, 5, 610, DateTimeKind.Unspecified).AddTicks(2746), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "admin@komsync.local", "System Admin", "ADMIN@KOMSYNC.LOCAL", "$2a$11$FlSzLJ4dRThnNSEs/S0hBej69RsK2DYPM0jaOsCUrBFilURG.7pma", new Guid("00000000-0000-0000-0000-000000000000"), 3, null });

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectHistories_Users_ChangedById",
                table: "ProjectHistories",
                column: "ChangedById",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectHistories_Users_ChangedById",
                table: "ProjectHistories");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("10ca3dbd-eaa5-4361-9a0f-49be6bc8549d"));

            migrationBuilder.AlterColumn<Guid>(
                name: "ChangedById",
                table: "ProjectHistories",
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
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
