
namespace TodoList;
internal class UncorrectCommand : ICommand
{
	public void Execute()
	{
		Console.WriteLine("Не правильно введена комманда.");
	}
}
