using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoApp.Data.Migrations;

[DbContext(typeof(AppDbContext))]
[Migration("20260330090728_InitialCreate")]
public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Profiles",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                Login = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false, collation: "NOCASE"),
                Password = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                FirstName = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                LastName = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                BirthYear = table.Column<int>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Profiles", profile => profile.Id);
            });

        migrationBuilder.CreateTable(
            name: "Todos",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                Text = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                Status = table.Column<int>(type: "INTEGER", nullable: false),
                LastUpdate = table.Column<DateTime>(type: "TEXT", nullable: false),
                SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                ProfileId = table.Column<Guid>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Todos", todo => todo.Id);
                table.ForeignKey(
                    name: "FK_Todos_Profiles_ProfileId",
                    column: todo => todo.ProfileId,
                    principalTable: "Profiles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Profiles_Login",
            table: "Profiles",
            column: "Login",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Todos_ProfileId_SortOrder",
            table: "Todos",
            columns: new[] { "ProfileId", "SortOrder" });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "Todos");
        migrationBuilder.DropTable(name: "Profiles");
    }
}
