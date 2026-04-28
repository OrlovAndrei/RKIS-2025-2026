using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Todolist.Data;
using Todolist.Models;

namespace Todolist.Services
{
    public class TodoRepository
    {
        public List<TodoItem> GetAllByProfileId(Guid profileId)
        {
            using var context = new AppDbContext();
            return context.Todos
                .AsNoTracking()
                .Where(t => t.ProfileId == profileId)
                .OrderBy(t => t.Id)
                .ToList();
        }

        public TodoItem? GetById(int id)
        {
            using var context = new AppDbContext();
            return context.Todos.AsNoTracking().FirstOrDefault(t => t.Id == id);
        }

        public void Add(TodoItem item)
        {
            using var context = new AppDbContext();
            context.Todos.Add(item);
            context.SaveChanges();
        }

        public void Update(TodoItem item)
        {
            using var context = new AppDbContext();
            context.Todos.Update(item);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            using var context = new AppDbContext();
            var todo = context.Todos.Find(id);
            if (todo != null)
            {
                context.Todos.Remove(todo);
                context.SaveChanges();
            }
        }

        public void SetStatus(int id, TodoStatus status)
        {
            using var context = new AppDbContext();
            var todo = context.Todos.Find(id);
            if (todo != null)
            {
                todo.SetStatus(status);
                context.SaveChanges();
            }
        }
    }
}