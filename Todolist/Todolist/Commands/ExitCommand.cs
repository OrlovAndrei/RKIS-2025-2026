namespace TodoList.Commands;

public class ExitCommand : ICommand
{
    public void Execute()
    {
        Console.WriteLine("Программа завершена.");
        Environment.Exit(0);
    }
}