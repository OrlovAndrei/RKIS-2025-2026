using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TodoApp.Data;
using TodoApp.Models;

namespace TodoApp.Services
{
    public class ProfileRepository
    {
        public List<Profile> GetAll()
        {
            using var context = new AppDbContext();
            return context.Profiles
                .AsNoTracking()
                .OrderBy(profile => profile.Login)
                .ToList();
        }

        public Profile? GetById(Guid id)
        {
            using var context = new AppDbContext();
            return context.Profiles
                .AsNoTracking()
                .FirstOrDefault(profile => profile.Id == id);
        }

        public Profile? GetByLogin(string login)
        {
            using var context = new AppDbContext();
            return context.Profiles
                .AsNoTracking()
                .FirstOrDefault(profile => profile.Login.ToLower() == login.ToLower());
        }

        public void Add(Profile profile)
        {
            using var context = new AppDbContext();
            context.Profiles.Add(profile);
            context.SaveChanges();
        }

        public void Update(Profile profile)
        {
            using var context = new AppDbContext();
            context.Profiles.Update(profile);
            context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            using var context = new AppDbContext();
            var profile = context.Profiles.FirstOrDefault(item => item.Id == id);
            if (profile == null)
            {
                return;
            }

            context.Profiles.Remove(profile);
            context.SaveChanges();
        }

        public void SaveAll(IEnumerable<Profile> profiles)
        {
            using var context = new AppDbContext();
            var incoming = profiles.ToList();
            var incomingIds = incoming.Select(profile => profile.Id).ToHashSet();
            var existingProfiles = context.Profiles.ToList();

            foreach (var existing in existingProfiles.Where(profile => !incomingIds.Contains(profile.Id)))
            {
                context.Profiles.Remove(existing);
            }

            foreach (var profile in incoming)
            {
                var existing = existingProfiles.FirstOrDefault(item => item.Id == profile.Id);
                if (existing == null)
                {
                    context.Profiles.Add(Clone(profile));
                }
                else
                {
                    existing.Login = profile.Login;
                    existing.Password = profile.Password;
                    existing.FirstName = profile.FirstName;
                    existing.LastName = profile.LastName;
                    existing.BirthYear = profile.BirthYear;
                }
            }

            context.SaveChanges();
        }

        private Profile Clone(Profile profile)
        {
            return new Profile
            {
                Id = profile.Id,
                Login = profile.Login,
                Password = profile.Password,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                BirthYear = profile.BirthYear
            };
        }
    }
}
