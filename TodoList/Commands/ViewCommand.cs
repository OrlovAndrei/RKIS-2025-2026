using TodoList;
namespace TodoApp.Commands;
public class ViewCommand : BaseCommand
{
	public TodoList TodoList { get; set; }
	public bool ShowIndex { get; set; }
	public bool ShowStatus { get; set; }
	public bool ShowDate { get; set; }
	public bool ShowAll { get; set; }

	public ViewCommand()
	{
		TodoList = AppInfo.Todos;
	}

	public override void Execute()
	{
		if (ShowAll)
			TodoList.View(true, true, true);
		else
			TodoList.View(ShowIndex, ShowStatus, ShowDate);
	}

	public override void Unexecute()
	{
		Console.WriteLine("Отмена просмотра списка (нет изменений для отмены)");
	}
}

