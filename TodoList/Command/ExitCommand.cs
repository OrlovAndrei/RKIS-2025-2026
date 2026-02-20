using System;

public class ExitCommand : ICommand
{
    public void Execute()
    {
        System.Environment.Exit(0);
    }
}