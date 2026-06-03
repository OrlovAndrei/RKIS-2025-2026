using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TodoApp.Models;

namespace TodoApp.Data
{
    public class TodoRepository
    {
        public List<TodoItem> GetAll(Guid profileId)
        {
            using var context = new AppDbContext();
            return context.Todos.AsNoTracking()
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
            var existing = context.Todos.FirstOrDefault(todo => todo.Id == item.Id);
            if (existing == null)
            {
                return;
            }

            existing.Text = item.Text;
            existing.Status = item.Status;
            existing.LastUpdate = DateTime.Now;
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

            todo.Status = status;
            todo.LastUpdate = DateTime.Now;
            context.SaveChanges();
        }
    }
}
