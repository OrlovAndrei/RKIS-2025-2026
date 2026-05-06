using System;
using System.Collections.Generic;
using TodoList.Exceptions;
using TodoList.Models;
using TodoList.Data;

namespace TodoList.Commands
{
    public class AddCommand : ICommand, IUndo, IRepositoryCommand
    {
        public string Text { get; set; } = string.Empty;
        public bool IsMultiline { get; set; }
        public string[] Flags { get; set; } = Array.Empty<string>();

        private IProfileRepository _profileRepository = null!;
        private ITodoRepository _todoRepository = null!;

        private TodoItem? _addedItem;
        private int _addedIndex;

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
                string text = GetTaskText();
                var newItem = new TodoItem(text)
                {
                    ProfileId = currentProfileId.Value
                };

                _addedItem = _todoRepository.AddAsync(newItem).Result;
                RefreshCurrentUserTodos(currentProfileId.Value);

                var tasks = AppInfo.CurrentUserTodos?.GetAllTasks();
                _addedIndex = tasks?.FindIndex(task => task.Id == _addedItem.Id) ?? -1;

                Console.WriteLine("Задача добавлена!");
                return;
            }

            var todos = AppInfo.CurrentUserTodos;
            if (todos == null)
            {
                throw new AuthenticationException("Не удалось получить список задач. Войдите в профиль.");
            }

            if (IsMultiline)
            {
                todos.AddTask("", Flags);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(Text))
                {
                    throw new InvalidArgumentException("Не введён текст задачи.");
                }

                todos.AddTask(Text, Flags);
            }

            _addedIndex = todos.GetAllTasks().Count - 1;
            _addedItem = todos.GetAllTasks()[_addedIndex];

            Program.SaveCurrentUserTasks();
        }

        public void Unexecute()
        {
            if (_addedItem == null)
            {
                return;
            }

            if (Program.UseDatabase && _todoRepository != null)
            {
                _todoRepository.DeleteAsync(_addedItem.Id, _addedItem.ProfileId).Wait();
                RefreshCurrentUserTodos(_addedItem.ProfileId);
                Console.WriteLine("Добавление задачи отменено");
                return;
            }

            var todos = AppInfo.CurrentUserTodos;
            if (todos != null)
            {
                var tasks = todos.GetAllTasks();
                if (_addedIndex >= 0 && _addedIndex < tasks.Count)
                {
                    tasks.RemoveAt(_addedIndex);
                }
                else
                {
                    tasks.Remove(_addedItem);
                }

                Console.WriteLine("Добавление задачи отменено");
                Program.SaveCurrentUserTasks();
            }
        }

        private string GetTaskText()
        {
            if (IsMultiline)
            {
                Console.WriteLine("Многострочный ввод (введите !end для завершения):");
                var lines = new List<string>();

                while (true)
                {
                    Console.Write("> ");
                    string? input = Console.ReadLine();
                    if (input == null || input.Trim() == "!end")
                    {
                        break;
                    }

                    if (!string.IsNullOrWhiteSpace(input))
                    {
                        lines.Add(input);
                    }
                }

                if (lines.Count == 0)
                {
                    throw new InvalidArgumentException("Не введён текст задачи.");
                }

                return string.Join("\n", lines);
            }

            if (string.IsNullOrWhiteSpace(Text))
            {
                throw new InvalidArgumentException("Не введён текст задачи.");
            }

            string text = Text.Trim('"', '\'');
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new InvalidArgumentException("Текст задачи не может быть пустым.");
            }

            return text;
        }

        private void RefreshCurrentUserTodos(Guid profileId)
        {
            var todos = AppInfo.CurrentUserTodos;
            if (todos == null)
            {
                return;
            }

            var tasks = todos.GetAllTasks();
            tasks.Clear();
            tasks.AddRange(_todoRepository.GetAllAsync(profileId).Result);
        }
    }
}
