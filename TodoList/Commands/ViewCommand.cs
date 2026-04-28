
namespace TodoList;
internal class ViewCommand : ICommand
{
	private bool _showIndexes;
	private bool _showDates;
	private bool _showStatuses;

	public ViewCommand(bool indexes, bool statuses, bool dates)
	{
		_showDates = dates;
		_showStatuses = statuses;
		_showIndexes = indexes;
	}

	public void Execute()
	{
		Console.WriteLine("===========================================================");
		Console.WriteLine("\tИнформация о задачах\t");
		AppInfo.CurrentTodoList.View(_showIndexes, _showStatuses, _showDates);
		Console.WriteLine("===========================================================");
	}

}
