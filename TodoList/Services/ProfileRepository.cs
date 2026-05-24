using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Todolist.Data;
using Todolist.Models;

namespace Todolist.Services
{
    public class ProfileRepository
    {
        public List<Profile> GetAll()
        {
            using var context = new AppDbContext();
            return context.Profiles.AsNoTracking().ToList();
        }

        public Profile? GetById(Guid id)
        {
            using var context = new AppDbContext();
            return context.Profiles.AsNoTracking().FirstOrDefault(p => p.Id == id);
        }

        public Profile? GetByLogin(string login)
        {
            using var context = new AppDbContext();
            return context.Profiles.AsNoTracking().FirstOrDefault(p => p.Login == login);
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
            var profile = context.Profiles.Find(id);
            if (profile != null)
            {
                context.Profiles.Remove(profile);
                context.SaveChanges();
            }
        }

        public bool LoginExists(string login)
        {
            using var context = new AppDbContext();
            return context.Profiles.Any(p => p.Login == login);
        }
    }
}