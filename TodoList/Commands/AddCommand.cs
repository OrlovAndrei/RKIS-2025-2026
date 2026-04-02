using System;

namespace TodoList;

public class AddCommand : ICommand
{
    private readonly TodoList _todoList;
    private readonly string _initialText;
    private readonly bool _isMultiline;
    private readonly string _todoFilePath;

    public AddCommand(TodoList todoList, string initialText, bool isMultiline, string todoFilePath)
    {
        _todoList = todoList;
        _initialText = initialText;
        _isMultiline = isMultiline;
        _todoFilePath = todoFilePath;
    }

    public void Execute()
    {
        string finalText = _initialText;
        if (_isMultiline)
        {
            Console.WriteLine("Многострочный ввод, введите !end для завершения");
            finalText = "";
            while (true)
            {
                string line = Console.ReadLine();
                if (line == "!end") break;
                finalText += line + "\n";
            }
        }

        _todoList.Add(new TodoItem(finalText));
        Console.WriteLine($"Задача добавлена: {finalText}");
        FileManager.SaveTodos(_todoList, _todoFilePath);
    }
}