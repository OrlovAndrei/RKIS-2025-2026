namespace TodoList.Commands;

public class UnknownCommand : ICommand
{
    public void Execute()
    {
        Console.WriteLine("Неизвестная команда. Введите 'help' для списка команд.");
    }
}