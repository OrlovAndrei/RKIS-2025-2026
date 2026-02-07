using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShevricTodo.Migrations
{
    /// <inheritdoc />
    public partial class test02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_StateOfTask_StateId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_TypeOfTasks_TypeId",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TypeOfTasks",
                table: "TypeOfTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StateOfTask",
                table: "StateOfTask");

            migrationBuilder.RenameTable(
                name: "TypeOfTasks",
                newName: "TypesOfTasks");

            migrationBuilder.RenameTable(
                name: "StateOfTask",
                newName: "StatesOfTask");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfCreate",
                table: "Tasks",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TypesOfTasks",
                table: "TypesOfTasks",
                column: "TypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StatesOfTask",
                table: "StatesOfTask",
                column: "StateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_StatesOfTask_StateId",
                table: "Tasks",
                column: "StateId",
                principalTable: "StatesOfTask",
                principalColumn: "StateId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_TypesOfTasks_TypeId",
                table: "Tasks",
                column: "TypeId",
                principalTable: "TypesOfTasks",
                principalColumn: "TypeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_StatesOfTask_StateId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_TypesOfTasks_TypeId",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TypesOfTasks",
                table: "TypesOfTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StatesOfTask",
                table: "StatesOfTask");

            migrationBuilder.RenameTable(
                name: "TypesOfTasks",
                newName: "TypeOfTasks");

            migrationBuilder.RenameTable(
                name: "StatesOfTask",
                newName: "StateOfTask");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfCreate",
                table: "Tasks",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TypeOfTasks",
                table: "TypeOfTasks",
                column: "TypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StateOfTask",
                table: "StateOfTask",
                column: "StateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_StateOfTask_StateId",
                table: "Tasks",
                column: "StateId",
                principalTable: "StateOfTask",
                principalColumn: "StateId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_TypeOfTasks_TypeId",
                table: "Tasks",
                column: "TypeId",
                principalTable: "TypeOfTasks",
                principalColumn: "TypeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
