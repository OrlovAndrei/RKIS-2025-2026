using System;
using System.Collections.Generic;
using System.IO;
using TodoApp.Commands;

namespace TodoApp;

public class CommandParser
{
    private TodoList todoList;
    private Profile profile;
    private List<ICommand> availableCommands;

    // Пути к файлам
    private readonly string todosFilePath = Path.Combine("data", "todo.csv");
    private readonly string profileFilePath = Path.Combine("data", "profile.txt");

    public CommandParser(TodoList todoList, Profile profile)
    {
        this.todoList = todoList;
        this.profile = profile;
        InitializeCommands();
    }

    private void InitializeCommands()
    {
        availableCommands = new List<ICommand>
        {
            new HelpCommand { AvailableCommands = new List<ICommand>() },
            new AddCommand { TodoList = todoList, TodoFilePath = todosFilePath },
            new ViewCommand { TodoList = todoList },
            new DoneCommand { TodoList = todoList, TodoFilePath = todosFilePath },
            new UpdateCommand { TodoList = todoList, TodoFilePath = todosFilePath },
            new ReadCommand { TodoList = todoList },
            new ModifyCommand { UserProfile = profile, ProfileFilePath = profileFilePath },
            new RemoveCommand { TodoList = todoList, TodoFilePath = todosFilePath },
            new ExitCommand { 
                TodoList = todoList, 
                UserProfile = profile,
                TodoFilePath = todosFilePath,
                ProfileFilePath = profileFilePath
            }
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
            return new HelpCommand { AvailableCommands = availableCommands };
        }

        ParseArguments(command, parts);
        return command;
    }

    private ICommand CreateCommandInstance(string commandName)
    {
        return commandName switch
        {
            "help" => new HelpCommand { AvailableCommands = availableCommands },
            "add" => new AddCommand { TodoList = todoList, TodoFilePath = todosFilePath },
            "view" => new ViewCommand { TodoList = todoList },
            "done" => new DoneCommand { TodoList = todoList, TodoFilePath = todosFilePath },
            "update" => new UpdateCommand { TodoList = todoList, TodoFilePath = todosFilePath },
            "read" => new ReadCommand { TodoList = todoList },
            "modify" => new ModifyCommand { UserProfile = profile, ProfileFilePath = profileFilePath },
            "remove" => new RemoveCommand { TodoList = todoList, TodoFilePath = todosFilePath },
            "exit" => new ExitCommand { 
                TodoList = todoList, 
                UserProfile = profile,
                TodoFilePath = todosFilePath,
                ProfileFilePath = profileFilePath
            },
            _ => new HelpCommand { AvailableCommands = availableCommands }
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
            case DoneCommand doneCommand:
                ParseDoneCommand(doneCommand, parts);
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

    private void ParseDoneCommand(DoneCommand command, string[] parts)
    {
        if (parts.Length > 1 && int.TryParse(parts[1], out int index))
        {
            command.TaskIndex = index;
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
