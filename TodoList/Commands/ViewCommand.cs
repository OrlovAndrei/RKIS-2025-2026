using System;
using System.Collections.Generic;
using TodoList.Models;
using TodoList.Services;

namespace TodoList.Commands
{
    public class ViewCommand : ICommand, IRepositoryCommand
    {
        public string[] Flags { get; set; } = Array.Empty<string>();

        private IProfileRepository _profileRepository = null!;
        private ITodoRepository _todoRepository = null!;

        public void SetRepositories(IProfileRepository profileRepository, ITodoRepository todoRepository)
        {
            _profileRepository = profileRepository;
            _todoRepository = todoRepository;
        }

        public void Execute()
        {
            var currentProfileId = AppInfo.CurrentProfileId;
            if (currentProfileId == null)
            {
                Console.WriteLine("Ошибка: не удалось получить список задач. Войдите в профиль.");
                return;
            }

            if (Program.UseDatabase && _todoRepository != null)
            {
                var todosFromRepository = LoadTodosFromRepository(currentProfileId.Value);
                todosFromRepository.ViewTasks(Flags);
                return;
            }

            var todos = AppInfo.CurrentUserTodos;
            if (todos == null)
            {
                Console.WriteLine("Ошибка: не удалось получить список задач. Войдите в профиль.");
                return;
            }

            todos.ViewTasks(Flags);
        }

        public void Unexecute() { }

        private global::TodoList.TodoList LoadTodosFromRepository(Guid profileId)
        {
            var tasks = new List<TodoItem>(_todoRepository.GetAllAsync(profileId).Result);
            var cachedTodos = AppInfo.CurrentUserTodos;

            if (cachedTodos != null)
            {
                var cachedTasks = cachedTodos.GetAllTasks();
                cachedTasks.Clear();
                cachedTasks.AddRange(tasks);
            }

            return new global::TodoList.TodoList(tasks);
        }
    }
}
