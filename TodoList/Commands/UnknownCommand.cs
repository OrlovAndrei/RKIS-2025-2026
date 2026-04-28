namespace TodoList.Commands;

public class UnknownCommand : ICommand
{
	public void Execute()
	{
		Console.WriteLine("Введена неизвестная команда. Используйте help");
	}
}