using TodoApp.Models;

namespace TodoApp.Desktop.Services;

public sealed class ProfileSessionService
{
    public Profile? CurrentProfile { get; set; }

    public Profile RequireProfile()
    {
        return CurrentProfile ?? throw new InvalidOperationException("Профиль не выбран.");
    }
}
