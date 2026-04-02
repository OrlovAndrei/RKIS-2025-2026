using System;

namespace TodoList;

public class DoneCommand : ICommand
{
    private readonly TodoList _todoList;
    private readonly int _index;
    private readonly string _todoFilePath;

    public DoneCommand(TodoList todoList, int index, string todoFilePath)
    {
        _todoList = todoList;
        _index = index;
        _todoFilePath = todoFilePath;
    }

    public void Execute()
    {
        _todoList.MarkDone(_index);
        Console.WriteLine($"Задача {_index + 1} отмечена выполненной");
        FileManager.SaveTodos(_todoList, _todoFilePath);
    }
}