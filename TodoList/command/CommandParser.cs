using System;
using System.Collections.Generic;
using System.IO;
using TodoApp.Commands;

namespace TodoApp
{
    public class CommandParser
    {
        private List<ICommand> availableCommands;

        public CommandParser()
        {
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            availableCommands = new List<ICommand>
            {
                new HelpCommand(),
                new AddCommand(),
                new ViewCommand(),
                new StatusCommand(),
                new UpdateCommand(),
                new ReadCommand(),
                new ModifyCommand(),
                new RemoveCommand(),
                new UndoCommand(),
                new RedoCommand(),
                new ExitCommand()
            };

            // Устанавливаем список команд для HelpCommand
            var helpCommand = availableCommands.Find(c => c is HelpCommand) as HelpCommand;
            if (helpCommand != null)
            {
                helpCommand.AvailableCommands = availableCommands;
            }
        }

        public ICommand Parse(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return null;

            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0)
                return null;

            string commandName = parts[0].ToLower();
            ICommand command = CreateCommandInstance(commandName);

            if (command == null)
            {
                Console.WriteLine($"❌ Неизвестная команда: {commandName}");
                Console.WriteLine("Введите 'help' для просмотра доступных команд.");
                return new HelpCommand();
            }

            ParseArguments(command, parts);
            return command;
        }

        private ICommand CreateCommandInstance(string commandName)
        {
            return commandName switch
            {
                "help" => new HelpCommand(),
                "add" => new AddCommand(),
                "view" => new ViewCommand(),
                "status" => new StatusCommand(),
                "update" => new UpdateCommand(),
                "read" => new ReadCommand(),
                "modify" => new ModifyCommand(),
                "remove" => new RemoveCommand(),
                "undo" => new UndoCommand(),
                "redo" => new RedoCommand(),
                "exit" => new ExitCommand(),
                _ => new HelpCommand()
            };
        }

        private void ParseArguments(ICommand command, string[] parts)
        {
            switch (command)
            {
                case AddCommand addCommand:
                    ParseAddCommand(addCommand, parts);
                    break;
                case ViewCommand viewCommand:
                    ParseViewCommand(viewCommand, parts);
                    break;
                case StatusCommand statusCommand:
                    ParseStatusCommand(statusCommand, parts);
                    break;
                case UpdateCommand updateCommand:
                    ParseUpdateCommand(updateCommand, parts);
                    break;
                case ReadCommand readCommand:
                    ParseReadCommand(readCommand, parts);
                    break;
                case RemoveCommand removeCommand:
                    ParseRemoveCommand(removeCommand, parts);
                    break;
                case ModifyCommand modifyCommand:
                    // ModifyCommand не имеет аргументов
                    break;
                case UndoCommand undoCommand:
                    // UndoCommand не имеет аргументов
                    break;
                case RedoCommand redoCommand:
                    // RedoCommand не имеет аргументов
                    break;
                case ExitCommand exitCommand:
                    // ExitCommand не имеет аргументов
                    break;
                case HelpCommand helpCommand:
                    // HelpCommand не имеет аргументов
                    break;
            }
        }

        private void ParseAddCommand(AddCommand command, string[] parts)
        {
            command.Multiline = false;
            command.TaskText = "";

            for (int i = 1; i < parts.Length; i++)
            {
                if (parts[i] == "-m" || parts[i] == "--multiline")
                {
                    command.Multiline = true;
                }
                else
                {
                    // Собираем оставшиеся части как текст задачи
                    List<string> textParts = new List<string>();
                    for (int j = i; j < parts.Length; j++)
                    {
                        textParts.Add(parts[j]);
                    }
                    command.TaskText = string.Join(" ", textParts);
                    break;
                }
            }
        }

        private void ParseViewCommand(ViewCommand command, string[] parts)
        {
            command.ShowIndex = true;
            command.ShowStatus = true;
            command.ShowDate = true;

            for (int i = 1; i < parts.Length; i++)
            {
                if (parts[i] == "-i")
                {
                    command.ShowIndex = true;
                }
                else if (parts[i] == "-s")
                {
                    command.ShowStatus = false;
                }
                else if (parts[i] == "-d")
                {
                    command.ShowDate = false;
                }
                else if (parts[i].StartsWith("-") && parts[i].Length > 1)
                {
                    // Обработка комбинированных флагов типа -is, -sd и т.д.
                    string flags = parts[i].Substring(1);
                    foreach (char flag in flags)
                    {
                        if (flag == 'i') command.ShowIndex = true;
                        if (flag == 's') command.ShowStatus = false;
                        if (flag == 'd') command.ShowDate = false;
                    }
                }
            }
        }

        private void ParseStatusCommand(StatusCommand command, string[] parts)
        {
            if (parts.Length > 2 && int.TryParse(parts[1], out int index))
            {
                command.TaskIndex = index;
                string statusStr = parts[2].ToLower();
                
                // Парсим строку в enum
                command.Status = statusStr switch
                {
                    "notstarted" or "не начата" => TodoStatus.NotStarted,
                    "inprogress" or "в процессе" => TodoStatus.InProgress,
                    "completed" or "выполнена" => TodoStatus.Completed,
                    "postponed" or "отложена" => TodoStatus.Postponed,
                    "failed" or "провалена" => TodoStatus.Failed,
                    _ => TodoStatus.NotStarted // Значение по умолчанию
                };
            }
        }

        private void ParseUpdateCommand(UpdateCommand command, string[] parts)
        {
            if (parts.Length > 2 && int.TryParse(parts[1], out int index))
            {
                command.TaskIndex = index;
                // Собираем оставшийся текст как новый текст задачи
                List<string> textParts = new List<string>();
                for (int i = 2; i < parts.Length; i++)
                {
                    textParts.Add(parts[i]);
                }
                string newText = string.Join(" ", textParts);

                // Убираем кавычки если они есть
                if (newText.StartsWith("\"") && newText.EndsWith("\""))
                {
                    newText = newText.Substring(1, newText.Length - 2);
                }
                command.NewText = newText;
            }
        }

        private void ParseReadCommand(ReadCommand command, string[] parts)
        {
            if (parts.Length > 1 && int.TryParse(parts[1], out int index))
            {
                command.TaskIndex = index;
            }
        }

        private void ParseRemoveCommand(RemoveCommand command, string[] parts)
        {
            command.Force = false;

            for (int i = 1; i < parts.Length; i++)
            {
                if (parts[i] == "-f" || parts[i] == "--force")
                {
                    command.Force = true;
                }
                else if (int.TryParse(parts[i], out int index))
                {
                    command.TaskIndex = index;
                }
            }
        }
    }
}
