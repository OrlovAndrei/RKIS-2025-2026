using TodoApp;

namespace TodoApp.Commands;
public class ReadCommand : BaseCommand
{
	public TodoList TodoList { get; set; }
	public int Index { get; set; }
	public ReadCommand()
	{
		TodoList = AppInfo.Todos;
	}

	public ReadCommand(int index) : this()
	{
		Index = index;
	}

	public override void Execute()
	{
		var item = TodoList.GetItem(Index);
		if (item != null)
			Console.WriteLine(item.GetFullInfo());
	}

	public override void Unexecute()
	{
		Console.WriteLine("Отмена просмотра задачи (нет изменений для отмены)");
	}
}
