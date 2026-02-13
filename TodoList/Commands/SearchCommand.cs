
namespace TodoList.Commands;

internal class SearchCommand : ICommand
{
	private string _startWith = "";
	private string _endWith = "";
	private string _conteins = "";

	private DateTime _fromDate = DateTime.MinValue;
	private DateTime _toDate = DateTime.MaxValue;

	private bool _isStatusFlag = false;
	private TodoStatus _status = TodoStatus.NotStarted;

	private bool _sortText = false;
	private bool _sortDate = false;
	private bool _sortDesc = false;

	private bool _topFlag = false;
	private uint _topValue = 0;

	public SearchCommand() { }

	public void SetupStartWith(string value) => _startWith = value;
	public void SetupEndWith(string value) => _endWith = value;
	public void SetupContains(string value) => _conteins = value;
	public void SetFrom(DateTime value) => _fromDate = value;
	public void SetTo(DateTime value) => _toDate = value;
	public void SetStatus(TodoStatus value)
	{
		_isStatusFlag = true;
		_status = value;
	}

	public void Top(uint value) => _topValue = value;

	private bool CheckStatus(TodoItem item)
	{
		if (_isStatusFlag)
			return _status == item.Status;
		else return true;
	}

	private IOrderedEnumerable<TodoItem> GetSelected()
	{
		// TODO: ordering by flags
		IOrderedEnumerable<TodoItem> selected = from c in AppInfo.CurrentTodoList.Items
												where
												 (_startWith == "" || c.Text.StartsWith(_startWith)) &&
												  (_conteins == "" || c.Text.Contains(_conteins)) &&
												  (_endWith == "" || c.Text.EndsWith(_endWith)) &&
												  c.LastUpdate.Date >= _fromDate.Date &&
												  c.LastUpdate.Date <= _toDate.Date &&
												CheckStatus(c)
												orderby c.Text
												select c;

		return selected;
	}

	public void Execute()
	{
		IOrderedEnumerable<TodoItem> selected = GetSelected();

		Console.WriteLine("================================================");

		// Descriptions
		{
			string s = "";

			s += "Index\t";
			s += "Text\t\t\t\t";
			s += "Status\t\t";
			s += "LastUpdate";

			Console.WriteLine(s);
		}

		// Tasks texts
		{
			int index = 0;
			foreach (var i in selected)
			{

				if (_topFlag && index >= _topValue)
					break;

				Console.Write($"{index}\t");
				Console.Write($"{i.Text}\t");
				Console.Write($"{i.Status}\t\t");
				Console.Write($"{i.LastUpdate}\n");
				index++;
			}
		}

		Console.WriteLine("================================================");
	}

	public void Unexecute() { }
}
