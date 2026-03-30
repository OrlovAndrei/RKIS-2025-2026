using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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
            .OrderBy(p => p.Login)
            .Select(p => CloneProfile(p))
            .ToList();
    }

    public Profile? GetById(Guid id)
    {
        using AppDbContext context = _contextFactory();
        Profile? profile = context.Profiles
            .AsNoTracking()
            .FirstOrDefault(p => p.Id == id);

        return profile == null ? null : CloneProfile(profile);
    }

    public void Add(Profile profile)
    {
        if (profile == null) throw new ArgumentNullException(nameof(profile));

        using AppDbContext context = _contextFactory();
        context.Profiles.Add(CloneProfile(profile));
        context.SaveChanges();
    }

    public void Update(Profile profile)
    {
        if (profile == null) throw new ArgumentNullException(nameof(profile));

        using AppDbContext context = _contextFactory();
        Profile? existing = context.Profiles.FirstOrDefault(p => p.Id == profile.Id);
        if (existing == null)
        {
            context.Profiles.Add(CloneProfile(profile));
        }
        else
        {
            CopyProfile(existing, profile);
        }

        context.SaveChanges();
    }

    public void Delete(Guid profileId)
    {
        if (profileId == Guid.Empty) return;

        using AppDbContext context = _contextFactory();
        Profile? existing = context.Profiles.FirstOrDefault(p => p.Id == profileId);
        if (existing == null) return;

        context.Profiles.Remove(existing);
        context.SaveChanges();
    }

    public void ReplaceAll(IEnumerable<Profile> profiles)
    {
        if (profiles == null) throw new ArgumentNullException(nameof(profiles));

        List<Profile> incoming = profiles
            .Select(CloneProfile)
            .ToList();
        var incomingIds = incoming.Select(p => p.Id).ToHashSet();

        using AppDbContext context = _contextFactory();
        List<Profile> existingProfiles = context.Profiles.ToList();
        var existingById = existingProfiles.ToDictionary(p => p.Id);

        foreach (Profile existing in existingProfiles.Where(p => !incomingIds.Contains(p.Id)))
        {
            context.Profiles.Remove(existing);
        }

        foreach (Profile profile in incoming)
        {
            if (existingById.TryGetValue(profile.Id, out Profile? existing))
            {
                CopyProfile(existing, profile);
            }
            else
            {
                context.Profiles.Add(profile);
            }
        }

        context.SaveChanges();
    }

    private static Profile CloneProfile(Profile source)
    {
        return new Profile(
            source.Id,
            source.Login ?? string.Empty,
            source.Password ?? string.Empty,
            source.FirstName ?? string.Empty,
            source.LastName ?? string.Empty,
            source.BirthYear);
    }

    private static void CopyProfile(Profile target, Profile source)
    {
        target.Login = source.Login ?? string.Empty;
        target.Password = source.Password ?? string.Empty;
        target.FirstName = source.FirstName ?? string.Empty;
        target.LastName = source.LastName ?? string.Empty;
        target.BirthYear = source.BirthYear;
    }
}
