using Microsoft.EntityFrameworkCore;

namespace TodoList.Data;

public static class DatabaseInitializer
{
    public static async Task InitializeAsync()
    {
        await using var context = new AppDbContext();
        await context.Database.MigrateAsync();
    }
}
