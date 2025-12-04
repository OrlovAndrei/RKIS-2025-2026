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
        /// <returns>Объект команды, реализующий ICommand</returns>
        public static ICommand Parse(string inputString)
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
                "profile" => CreateProfileCommand(parts),
                "add" => CreateAddCommand(parts),
                "view" => CreateViewCommand(parts),
                "status" => CreateStatusCommand(parts),
                "delete" => CreateDeleteCommand(parts),
                "update" => CreateUpdateCommand(parts),
                "read" => CreateReadCommand(parts),
                "undo" => new UndoCommand(),
                "redo" => new RedoCommand(),
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

        private static ICommand CreateProfileCommand(string[] parts)
        {
            var command = new ProfileCommand();

            // Проверяем флаги --out или -o
            if (parts.Contains("--out") || parts.Contains("-o"))
            {
                command.Logout = true;
            }

            return command;
        }

        private static ICommand CreateAddCommand(string[] parts)
        {
            var command = new AddCommand();

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

        private static ICommand CreateViewCommand(string[] parts)
        {
            var command = new ViewCommand();

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

        private static ICommand CreateStatusCommand(string[] parts)
        {
            var command = new StatusCommand();

            // Формат: status <idx> <status>
            if (parts.Length >= 2 && int.TryParse(parts[1], out int idx))
            {
                command.Index = idx;
            }

            if (parts.Length >= 3)
            {
                string statusText = parts[2].ToLowerInvariant();
                if (Enum.TryParse<TodoStatus>(statusText, ignoreCase: true, out var status))
                {
                    command.Status = status;
                }
                else
                {
                    Console.WriteLine("Некорректный статус. Допустимые значения: notstarted, inprogress, completed, postponed, failed");
                }
            }

            return command;
        }

        private static ICommand CreateDeleteCommand(string[] parts)
        {
            var command = new DeleteCommand();

            if (parts.Length >= 2 && int.TryParse(parts[1], out int idx))
            {
                command.Index = idx;
            }

            return command;
        }

        private static ICommand CreateUpdateCommand(string[] parts)
        {
            var command = new UpdateCommand();

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

        private static ICommand CreateReadCommand(string[] parts)
        {
            var command = new ReadCommand();

            if (parts.Length >= 2 && int.TryParse(parts[1], out int idx))
            {
                command.Index = idx;
            }

            return command;
        }

    }
}

