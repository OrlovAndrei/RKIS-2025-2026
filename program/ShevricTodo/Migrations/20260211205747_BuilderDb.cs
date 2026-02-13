using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ShevricTodo.Migrations;

/// <inheritdoc />
public partial class BuilderDb : Migration
{
	/// <inheritdoc />
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.CreateTable(
			name: "Profiles",
			columns: table => new
			{
				UserId = table.Column<int>(type: "INTEGER", nullable: false)
					.Annotation("Sqlite:Autoincrement", true),
				FirstName = table.Column<string>(type: "TEXT", nullable: true),
				LastName = table.Column<string>(type: "TEXT", nullable: true),
				UserName = table.Column<string>(type: "TEXT", nullable: true),
				DateOfCreate = table.Column<DateTime>(type: "TEXT", nullable: true),
				Birthday = table.Column<DateTime>(type: "TEXT", nullable: true),
				HashPassword = table.Column<string>(type: "TEXT", nullable: true)
			},
			constraints: table =>
			{
				table.PrimaryKey("PK_Profiles", x => x.UserId);
			});

		migrationBuilder.CreateTable(
			name: "StatesOfTask",
			columns: table => new
			{
				StateId = table.Column<int>(type: "INTEGER", nullable: false)
					.Annotation("Sqlite:Autoincrement", true),
				Name = table.Column<string>(type: "TEXT", nullable: true),
				Description = table.Column<string>(type: "TEXT", nullable: true)
			},
			constraints: table =>
			{
				table.PrimaryKey("PK_StatesOfTask", x => x.StateId);
			});

		migrationBuilder.CreateTable(
			name: "TypesOfTasks",
			columns: table => new
			{
				TypeId = table.Column<int>(type: "INTEGER", nullable: false)
					.Annotation("Sqlite:Autoincrement", true),
				Name = table.Column<string>(type: "TEXT", nullable: true),
				Description = table.Column<string>(type: "TEXT", nullable: true)
			},
			constraints: table =>
			{
				table.PrimaryKey("PK_TypesOfTasks", x => x.TypeId);
			});

		migrationBuilder.CreateTable(
			name: "Tasks",
			columns: table => new
			{
				TaskId = table.Column<int>(type: "INTEGER", nullable: false)
					.Annotation("Sqlite:Autoincrement", true),
				TypeId = table.Column<int>(type: "INTEGER", nullable: false),
				StateId = table.Column<int>(type: "INTEGER", nullable: false),
				UserId = table.Column<int>(type: "INTEGER", nullable: false),
				Name = table.Column<string>(type: "TEXT", nullable: true),
				Description = table.Column<string>(type: "TEXT", nullable: true),
				DateOfCreate = table.Column<DateTime>(type: "TEXT", nullable: true),
				DateOfStart = table.Column<DateTime>(type: "TEXT", nullable: true),
				DateOfEnd = table.Column<DateTime>(type: "TEXT", nullable: true),
				Deadline = table.Column<DateTime>(type: "TEXT", nullable: true)
			},
			constraints: table =>
			{
				table.PrimaryKey("PK_Tasks", x => x.TaskId);
				table.ForeignKey(
					name: "FK_Tasks_Profiles_UserId",
					column: x => x.UserId,
					principalTable: "Profiles",
					principalColumn: "UserId",
					onDelete: ReferentialAction.Cascade);
				table.ForeignKey(
					name: "FK_Tasks_StatesOfTask_StateId",
					column: x => x.StateId,
					principalTable: "StatesOfTask",
					principalColumn: "StateId",
					onDelete: ReferentialAction.Cascade);
				table.ForeignKey(
					name: "FK_Tasks_TypesOfTasks_TypeId",
					column: x => x.TypeId,
					principalTable: "TypesOfTasks",
					principalColumn: "TypeId",
					onDelete: ReferentialAction.Cascade);
			});

		migrationBuilder.InsertData(
			table: "StatesOfTask",
			columns: new[] { "StateId", "Description", "Name" },
			values: new object[,]
			{
				{ 1, "Задание существует, но не было начато", "Не начато" },
				{ 2, "Задание было начато и находится в процессе выполнения", "В процессе" },
				{ 3, "Задание успешно выполнено", "Выполнено" },
				{ 4, "Задание было отложено", "Отложено" },
				{ 5, "Задание провалено", "Провалено" }
			});

		migrationBuilder.InsertData(
			table: "TypesOfTasks",
			columns: new[] { "TypeId", "Description", "Name" },
			values: new object[,]
			{
				{ 1, "Я люблю huis", "test" },
				{ 2, "Я люблю huis", "test02" }
			});

		migrationBuilder.CreateIndex(
			name: "IX_Tasks_StateId",
			table: "Tasks",
			column: "StateId");

		migrationBuilder.CreateIndex(
			name: "IX_Tasks_TypeId",
			table: "Tasks",
			column: "TypeId");

		migrationBuilder.CreateIndex(
			name: "IX_Tasks_UserId",
			table: "Tasks",
			column: "UserId");
	}

	/// <inheritdoc />
	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable(
			name: "Tasks");

		migrationBuilder.DropTable(
			name: "Profiles");

		migrationBuilder.DropTable(
			name: "StatesOfTask");

		migrationBuilder.DropTable(
			name: "TypesOfTasks");
	}
}
