using System;

namespace TodoList;

public class UpdateCommand : ICommand
{
    private readonly TodoList _todoList;
    private readonly int _index;
    private readonly string _newText;
    private readonly string _todoFilePath;

    public UpdateCommand(TodoList todoList, int index, string newText, string todoFilePath)
    {
        _todoList = todoList;
        _index = index;
        _newText = newText;
        _todoFilePath = todoFilePath;
    }

    public void Execute()
    {
        _todoList.Update(_index, _newText);
        Console.WriteLine($"Задача {_index + 1} обновлена");
        FileManager.SaveTodos(_todoList, _todoFilePath);
    }
}