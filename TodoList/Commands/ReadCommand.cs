

namespace TodoList;
internal class ReadCommand : IndexedCommand
{
	public ReadCommand(int index) : base(index) { }

	protected override void SubExecute(TodoItem item)
	{
		Console.WriteLine("\n" + item.GetFullInfo());
	}

}
