using System;
using System.Collections.Generic;
using TodoList.Exceptions;
using TodoList.Models;
using TodoList.Data;

namespace TodoList.Commands
{
    public class ReadCommand : ICommand, IRepositoryCommand
    {
        public string Arg { get; set; } = string.Empty;

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
                throw new AuthenticationException("Не удалось получить список задач. Войдите в профиль.");
            }

            if (Program.UseDatabase && _todoRepository != null)
            {
                var todosFromRepository = LoadTodosFromRepository(currentProfileId.Value);
                todosFromRepository.ReadTask(Arg);
                return;
            }

            var todos = AppInfo.CurrentUserTodos;
            if (todos == null)
            {
                throw new AuthenticationException("Не удалось получить список задач. Войдите в профиль.");
            }

            todos.ReadTask(Arg);
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
