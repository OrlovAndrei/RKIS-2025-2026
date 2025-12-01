using System;
using System.Collections.Generic;
using System.Linq;

namespace TodoList
{
    /// <summary>
    /// Статический класс для парсинга строк команд и создания объектов команд.
    /// </summary>
    internal static class CommandParser
    {
        /// <summary>
        /// Парсит строку ввода и создает соответствующую команду.
        /// </summary>
        /// <param name="inputString">Строка ввода пользователя</param>
        /// <param name="todoList">Текущий список задач</param>
        /// <param name="profile">Профиль пользователя</param>
        /// <param name="todoFilePath">Путь к файлу задач</param>
        /// <param name="profileFilePath">Путь к файлу профиля</param>
        /// <returns>Объект команды, реализующий ICommand</returns>
        public static ICommand Parse(string inputString, TodoList todoList, Profile profile, string? todoFilePath = null, string? profileFilePath = null)
        {
            if (string.IsNullOrWhiteSpace(inputString))
            {
                return new HelpCommand();
            }

            string trimmed = inputString.Trim();
            string[] parts = SplitCommand(trimmed);
            
            if (parts.Length == 0)
            {
                return new HelpCommand();
            }

            string verb = parts[0].ToLowerInvariant();

            return verb switch
            {
                "help" => new HelpCommand(),
                "exit" => new ExitCommand(),
                "profile" => CreateProfileCommand(profile, profileFilePath),
                "add" => CreateAddCommand(parts, todoList, todoFilePath),
                "view" => CreateViewCommand(parts, todoList),
                "done" => CreateDoneCommand(parts, todoList, todoFilePath),
                "delete" => CreateDeleteCommand(parts, todoList, todoFilePath),
                "update" => CreateUpdateCommand(parts, todoList, todoFilePath),
                "read" => CreateReadCommand(parts, todoList),
                _ => new HelpCommand()
            };
        }

        private static string[] SplitCommand(string input)
        {
            var parts = new List<string>();
            bool inQuotes = false;
            var current = new System.Text.StringBuilder();

            foreach (char c in input)
            {
                if (c == '"')
                {
                    if (inQuotes)
                    {
                        // Закрывающая кавычка - сохраняем накопленный текст
                        if (current.Length > 0)
                        {
                            parts.Add(current.ToString());
                            current.Clear();
                        }
                    }
                    // Иначе открывающая кавычка - просто переключаем флаг
                    inQuotes = !inQuotes;
                }
                else if (c == ' ' && !inQuotes)
                {
                    // Пробел вне кавычек - разделитель
                    if (current.Length > 0)
                    {
                        parts.Add(current.ToString());
                        current.Clear();
                    }
                }
                else
                {
                    current.Append(c);
                }
            }

            if (current.Length > 0)
            {
                parts.Add(current.ToString());
            }

            return parts.ToArray();
        }

        private static ICommand CreateProfileCommand(Profile profile, string? profileFilePath)
        {
            return new ProfileCommand { Profile = profile, ProfileFilePath = profileFilePath };
        }

        private static ICommand CreateAddCommand(string[] parts, TodoList todoList, string? todoFilePath)
        {
            var command = new AddCommand { TodoList = todoList, TodoFilePath = todoFilePath };

            // Проверяем флаг --multiline
            if (parts.Contains("--multiline"))
            {
                command.Multiline = true;
            }
            else if (parts.Length >= 2)
            {
                // Текст задачи должен быть во второй части (после парсинга кавычки удаляются)
                command.TaskText = parts[1];
            }

            return command;
        }

        private static ICommand CreateViewCommand(string[] parts, TodoList todoList)
        {
            var command = new ViewCommand { TodoList = todoList };

            // Проверяем флаги
            if (parts.Contains("--no-index"))
            {
                command.ShowIndex = false;
            }

            if (parts.Contains("--no-done"))
            {
                command.ShowDone = false;
            }

            if (parts.Contains("--no-date"))
            {
                command.ShowDate = false;
            }

            return command;
        }

        private static ICommand CreateDoneCommand(string[] parts, TodoList todoList, string? todoFilePath)
        {
            var command = new DoneCommand { TodoList = todoList, TodoFilePath = todoFilePath };

            if (parts.Length >= 2 && int.TryParse(parts[1], out int idx))
            {
                command.Index = idx;
            }

            return command;
        }

        private static ICommand CreateDeleteCommand(string[] parts, TodoList todoList, string? todoFilePath)
        {
            var command = new DeleteCommand { TodoList = todoList, TodoFilePath = todoFilePath };

            if (parts.Length >= 2 && int.TryParse(parts[1], out int idx))
            {
                command.Index = idx;
            }

            return command;
        }

        private static ICommand CreateUpdateCommand(string[] parts, TodoList todoList, string? todoFilePath)
        {
            var command = new UpdateCommand { TodoList = todoList, TodoFilePath = todoFilePath };

            // Формат: update <idx> "text"
            if (parts.Length >= 2 && int.TryParse(parts[1], out int idx))
            {
                command.Index = idx;

                // Текст должен быть в третьей части (или объединение всех частей после индекса)
                if (parts.Length >= 3)
                {
                    command.NewText = string.Join(" ", parts.Skip(2));
                }
            }

            return command;
        }

        private static ICommand CreateReadCommand(string[] parts, TodoList todoList)
        {
            var command = new ReadCommand { TodoList = todoList };

            if (parts.Length >= 2 && int.TryParse(parts[1], out int idx))
            {
                command.Index = idx;
            }

            return command;
        }

    }
}

