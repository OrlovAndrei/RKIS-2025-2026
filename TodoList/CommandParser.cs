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
        private static Dictionary<string, Func<string, ICommand>> _commandHandlers = new Dictionary<string, Func<string, ICommand>>();

        static CommandParser()
        {
            Init();
        }

        /// <summary>
        /// Инициализирует словарь обработчиков команд.
        /// </summary>
        private static void Init()
        {
            _commandHandlers["help"] = ParseHelp;
            _commandHandlers["exit"] = ParseExit;
            _commandHandlers["profile"] = ParseProfile;
            _commandHandlers["add"] = ParseAdd;
            _commandHandlers["view"] = ParseView;
            _commandHandlers["status"] = ParseStatus;
            _commandHandlers["delete"] = ParseDelete;
            _commandHandlers["update"] = ParseUpdate;
            _commandHandlers["read"] = ParseRead;
            _commandHandlers["undo"] = ParseUndo;
            _commandHandlers["redo"] = ParseRedo;
        }

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
            var parts = trimmed.Split(' ', 2);
            string command = parts[0].ToLowerInvariant();
            string args = parts.Length > 1 ? parts[1] : "";

            if (_commandHandlers.TryGetValue(command, out var handler))
            {
                return handler(args);
            }

            Console.WriteLine("Неизвестная команда.");
            return new HelpCommand();
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

        private static ICommand ParseHelp(string args)
        {
            return new HelpCommand();
        }

        private static ICommand ParseExit(string args)
        {
            return new ExitCommand();
        }

        private static ICommand ParseUndo(string args)
        {
            return new UndoCommand();
        }

        private static ICommand ParseRedo(string args)
        {
            return new RedoCommand();
        }

        private static ICommand ParseProfile(string args)
        {
            var command = new ProfileCommand();
            string[] parts = SplitCommand(args);

            // Проверяем флаги --out или -o
            if (parts.Contains("--out") || parts.Contains("-o"))
            {
                command.Logout = true;
            }

            return command;
        }

        private static ICommand ParseAdd(string args)
        {
            var command = new AddCommand();
            string[] parts = SplitCommand(args);

            // Проверяем флаг --multiline
            if (parts.Contains("--multiline"))
            {
                command.Multiline = true;
            }
            else if (parts.Length > 0 && !string.IsNullOrWhiteSpace(parts[0]))
            {
                // Текст задачи должен быть в первой части (после парсинга кавычки удаляются)
                command.TaskText = parts[0];
            }

            return command;
        }

        private static ICommand ParseView(string args)
        {
            var command = new ViewCommand();
            string[] parts = SplitCommand(args);

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

        private static ICommand ParseStatus(string args)
        {
            var command = new StatusCommand();
            string[] parts = SplitCommand(args);

            // Формат: status <idx> <status>
            if (parts.Length >= 1 && int.TryParse(parts[0], out int idx))
            {
                command.Index = idx;
            }

            if (parts.Length >= 2)
            {
                string statusText = parts[1].ToLowerInvariant();
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

        private static ICommand ParseDelete(string args)
        {
            var command = new DeleteCommand();
            string[] parts = SplitCommand(args);

            if (parts.Length >= 1 && int.TryParse(parts[0], out int idx))
            {
                command.Index = idx;
            }

            return command;
        }

        private static ICommand ParseUpdate(string args)
        {
            var command = new UpdateCommand();
            string[] parts = SplitCommand(args);

            // Формат: update <idx> "text"
            if (parts.Length >= 1 && int.TryParse(parts[0], out int idx))
            {
                command.Index = idx;

                // Текст должен быть во второй части и далее (объединение всех частей после индекса)
                if (parts.Length >= 2)
                {
                    command.NewText = string.Join(" ", parts.Skip(1));
                }
            }

            return command;
        }

        private static ICommand ParseRead(string args)
        {
            var command = new ReadCommand();
            string[] parts = SplitCommand(args);

            if (parts.Length >= 1 && int.TryParse(parts[0], out int idx))
            {
                command.Index = idx;
            }

            return command;
        }

    }
}

