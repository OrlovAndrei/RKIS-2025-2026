using System;
using System.Linq;

namespace TodoList
{
    public static class CommandParser
    {
        private const string CommandHelp = "help";
        private const string CommandProfile = "profile";
        private const string CommandAdd = "add";
        private const string CommandDone = "done";
        private const string CommandUpdate = "update";
        private const string CommandView = "view";
        private const string CommandRead = "read";
        private const string CommandExit = "exit";

        /// <summary>
        /// </summary>
        /// <param name="inputString">Введенная строка команды.</param>
        /// <param name="todoList">Объект TodoList для команд, работающих с задачами.</param>
        /// <param name="profile">Объект Profile для команды profile.</param>
        /// <returns>Объект, реализующий ICommand, или null, если команда неизвестна.</returns>
        public static ICommand Parse(string inputString, TodoList todoList, Profile profile)
        {
            if (string.IsNullOrEmpty(inputString))
            {
                return null;
            }

            string[] parts = inputString.Trim().Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
            string commandName = parts[0].ToLowerInvariant();
            string args = parts.Length > 1 ? parts[1].Trim() : string.Empty;

            try
            {
                switch (commandName)
                {
                    case CommandHelp:
                        return new HelpCommand();

                    case CommandProfile:
                        return new ProfileCommand(profile);

                    case CommandExit:
                        return new ExitCommand();

                    case CommandAdd:
                        return ParseAddCommand(args, todoList);

                    case CommandView:
                        return ParseViewCommand(args, todoList);

                    case CommandDone:
                        return ParseSimpleIndexCommand<DoneCommand>(args, todoList);

                    case CommandRead:
                        return ParseSimpleIndexCommand<ReadCommand>(args, todoList);

                    case CommandUpdate:
                        return ParseUpdateCommand(args, todoList);

                    default:
                        Console.WriteLine($"Неизвестная команда: {commandName}. Введите help для списка команд.");
                        return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при разборе команды '{commandName}': {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// </summary>
        private static ICommand ParseAddCommand(string args, TodoList todoList)
        {
            var command = new AddCommand(todoList);
            string taskText = args;

            if (args.EndsWith(CommandFlags.FlagMultiline) || args.EndsWith(CommandFlags.FlagShortMultiline))
            {
                command.IsMultiline = true;

                if (args.EndsWith(CommandFlags.FlagMultiline))
                {
                    taskText = args.Substring(0, args.Length - CommandFlags.FlagMultiline.Length).Trim();
                }
                else
                {
                    taskText = args.Substring(0, args.Length - CommandFlags.FlagShortMultiline.Length).Trim();
                }
            }

            if (command.IsMultiline)
            {
                taskText = ReadMultilineInput(taskText);
            }

            command.TaskText = taskText;
            return command;
        }

        /// <summary>
        /// </summary>
        private static string ReadMultilineInput(string initialText)
        {
            Console.WriteLine("Введите задачу (для завершения введите !end):");
            System.Text.StringBuilder taskBuilder = new System.Text.StringBuilder();

            if (!string.IsNullOrEmpty(initialText))
            {
                taskBuilder.AppendLine(initialText);
            }

            while (true)
            {
                Console.Write("> ");
                string line = Console.ReadLine();
                if (line?.Trim().Equals("!end", StringComparison.OrdinalIgnoreCase) == true)
                {
                    break;
                }
                taskBuilder.AppendLine(line);
            }
            return taskBuilder.ToString().TrimEnd();
        }


        /// <summary>
        /// </summary>
        private static ICommand ParseViewCommand(string args, TodoList todoList)
        {
            var command = new ViewCommand(todoList);

            command.ShowIndex = args.Contains(CommandFlags.FlagIndex) || args.Contains(CommandFlags.FlagShortIndex);
            command.ShowStatus = args.Contains(CommandFlags.FlagStatus) || args.Contains(CommandFlags.FlagShortStatus);
            command.ShowDate = args.Contains(CommandFlags.FlagDate) || args.Contains(CommandFlags.FlagShortDate);
            command.ShowAll = args.Contains(CommandFlags.FlagAll) || args.Contains(CommandFlags.FlagShortAll);

            return command;
        }

        /// <summary>
        /// </summary>
        private static ICommand ParseSimpleIndexCommand<T>(string args, TodoList todoList) where T : IndexedCommandBase
        {
            if (string.IsNullOrEmpty(args) || !int.TryParse(args, out int index))
            {
                Console.WriteLine($"Неверный формат. Требуется индекс: {typeof(T).Name.Replace("Command", "").ToLowerInvariant()} <idx>");
                return null;
            }

            T command = (T)Activator.CreateInstance(typeof(T), todoList);
            command.Index = index;
            return command;
        }

        /// <summary>
        /// </summary>
        private static ICommand ParseUpdateCommand(string args, TodoList todoList)
        {
            string[] parts = args.Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 2 || !int.TryParse(parts[0], out int index) || string.IsNullOrEmpty(parts[1]))
            {
                Console.WriteLine("Неверный формат команды. Используйте: update <idx> \"новый текст\"");
                return null;
            }

            var command = new UpdateCommand(todoList)
            {
                Index = index,
                NewText = parts[1]
            };

            return command;
        }
    }
}