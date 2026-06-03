using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TodoApp.Data;
using TodoApp.Models;

namespace TodoApp.Services
{
    public class TodoRepository
    {
        public List<TodoItem> GetAll()
        {
            using var context = new AppDbContext();
            return context.Todos
                .AsNoTracking()
                .OrderBy(todo => todo.Id)
                .ToList();
        }

        public List<TodoItem> GetAll(Guid profileId)
        {
            using var context = new AppDbContext();
            return context.Todos
                .AsNoTracking()
                .Where(todo => todo.ProfileId == profileId)
                .OrderBy(todo => todo.Id)
                .ToList();
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
            var todo = context.Todos.FirstOrDefault(item => item.Id == id);
            if (todo == null)
            {
                return;
            }

            context.Todos.Remove(todo);
            context.SaveChanges();
        }

        public void SetStatus(int id, TodoStatus status)
        {
            using var context = new AppDbContext();
            var todo = context.Todos.FirstOrDefault(item => item.Id == id);
            if (todo == null)
            {
                return;
            }

            todo.SetStatus(status);
            context.SaveChanges();
        }

        public void ReplaceForProfile(Guid profileId, IEnumerable<TodoItem> todos)
        {
            using var context = new AppDbContext();
            var existing = context.Todos.Where(todo => todo.ProfileId == profileId).ToList();
            context.Todos.RemoveRange(existing);

            foreach (var todo in todos)
            {
                context.Todos.Add(new TodoItem(todo.Text)
                {
                    ProfileId = profileId,
                    Status = todo.Status,
                    LastUpdate = todo.LastUpdate
                });
            }

            context.SaveChanges();
        }
    }
}
