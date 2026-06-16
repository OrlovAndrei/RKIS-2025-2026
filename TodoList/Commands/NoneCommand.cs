

namespace TodoList;
internal class NoneCommand : ICommand
{
	public void Execute()
	{
		Console.WriteLine("Неизвестная комманда.");
	}
}
