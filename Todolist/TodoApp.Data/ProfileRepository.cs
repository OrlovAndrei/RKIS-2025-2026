using Microsoft.EntityFrameworkCore;
using TodoApp.Models;

namespace TodoApp.Data;

public sealed class ProfileRepository
{
    private readonly Func<AppDbContext> _contextFactory;

    public ProfileRepository(Func<AppDbContext>? contextFactory = null)
    {
        _contextFactory = contextFactory ?? (() => new AppDbContext());
    }

    public List<Profile> GetAll()
    {
        using AppDbContext context = _contextFactory();

        return context.Profiles
            .AsNoTracking()
            .OrderBy(profile => profile.Login)
            .ToList();
    }

    public Profile? GetByLogin(string login)
    {
        if (string.IsNullOrWhiteSpace(login))
        {
            return null;
        }

        using AppDbContext context = _contextFactory();

        return context.Profiles
            .AsNoTracking()
            .FirstOrDefault(profile => profile.Login == login.Trim());
    }

    public Profile? GetById(Guid profileId)
    {
        if (profileId == Guid.Empty)
        {
            return null;
        }

        using AppDbContext context = _contextFactory();

        return context.Profiles
            .AsNoTracking()
            .FirstOrDefault(profile => profile.Id == profileId);
    }

    public void Add(Profile profile)
    {
        ArgumentNullException.ThrowIfNull(profile);

        using AppDbContext context = _contextFactory();

        bool loginExists = context.Profiles.Any(existing => existing.Login == profile.Login);
        if (loginExists)
        {
            throw new InvalidOperationException("Профиль с таким логином уже существует.");
        }

        context.Profiles.Add(profile);
        context.SaveChanges();
    }
}
