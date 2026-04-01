using System;

namespace TodoList;

public class ExitCommand : ICommand
{
    private readonly Action _exitAction;

    public ExitCommand(Action exitAction)
    {
        _exitAction = exitAction;
    }

    public void Execute()
    {
        Console.WriteLine("Программа завершена.");
        _exitAction();
    }
}