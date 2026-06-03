using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TodoApp.Models;

namespace TodoApp.Data
{
    public class ProfileRepository
    {
        public List<Profile> GetAll()
        {
            using var context = new AppDbContext();
            return context.Profiles.AsNoTracking().OrderBy(profile => profile.Login).ToList();
        }

        public Profile? GetByLogin(string login)
        {
            using var context = new AppDbContext();
            return context.Profiles.AsNoTracking()
                .FirstOrDefault(profile => profile.Login.ToLower() == login.ToLower());
        }

        public Profile? GetById(Guid id)
        {
            using var context = new AppDbContext();
            return context.Profiles.AsNoTracking().FirstOrDefault(profile => profile.Id == id);
        }

        public void Add(Profile profile)
        {
            using var context = new AppDbContext();
            context.Profiles.Add(profile);
            context.SaveChanges();
        }

        public bool LoginExists(string login)
        {
            using var context = new AppDbContext();
            return context.Profiles.Any(profile => profile.Login.ToLower() == login.ToLower());
        }
    }
}
