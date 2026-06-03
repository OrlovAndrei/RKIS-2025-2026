using System;
using System.Collections.Generic;
using TodoApp.Models;

namespace TodoApp.Services
{
    public class DatabaseDataStorage : IDataStorage
    {
        private readonly ProfileRepository _profileRepository = new();
        private readonly TodoRepository _todoRepository = new();

        public void SaveProfiles(IEnumerable<Profile> profiles)
        {
            _profileRepository.SaveAll(profiles);
        }

        public IEnumerable<Profile> LoadProfiles()
        {
            return _profileRepository.GetAll();
        }

        public void SaveTodos(Guid userId, IEnumerable<TodoItem> todos)
        {
            _todoRepository.ReplaceForProfile(userId, todos);
        }

        public IEnumerable<TodoItem> LoadTodos(Guid userId)
        {
            return _todoRepository.GetAll(userId);
        }
    }
}
