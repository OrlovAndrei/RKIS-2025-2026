namespace TodoList.commands;

public class UnknownCommand : ICommand
{
    public void Execute()
    {
        Console.WriteLine("Неизвестная команда");
    }

    public void Unexecute()
    {
        // Команда unknown не требует отмены
    }
}