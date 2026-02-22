namespace TodoList.commands;

public class ExitCommand : ICommand
{
    public void Execute()
    {
        Console.WriteLine("Выход из программы. До свидания!");
        Environment.Exit(0);
    }

    public void Unexecute()
    {
        // Команда exit не требует отмены
    }
}