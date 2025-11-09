using TodoApp;
namespace TodoApp.Commands;
public class ViewCommand : BaseCommand
{
	public TodoList TodoList { get; set; }
	public bool ShowIndex { get; set; }
	public bool ShowStatus { get; set; }
	public bool ShowDate { get; set; }
	public bool ShowAll { get; set; }

	public override void Execute()
	{
		if (ShowAll)
			TodoList.View(true, true, true);
		else
			TodoList.View(ShowIndex, ShowStatus, ShowDate);
	}
}
