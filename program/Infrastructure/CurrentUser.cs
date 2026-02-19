using Application.Interfaces;

namespace Infrastructure;

public class CurrentUser : ICurrentUserService
{
    public Guid UserId { get; private set; }

    public void Clear()
    {
        UserId = Guid.Empty;
    }

    public void Set(Guid userId)
    {
        UserId = userId;
    }
}