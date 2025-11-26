using TodoApp;
namespace TodoApp.Commands;
public class UpdateCommand : BaseCommand
{
	public TodoList TodoList { get; set; }
	public int Index { get; set; }
	public string NewText { get; set; }
	private string _oldText;

	public UpdateCommand()
	{
		TodoList = AppInfo.Todos;
	}

	public UpdateCommand(int index, string newText) : this()
	{
		Index = index;
		NewText = newText;
	}

	public override void Execute()
	{
		var item = TodoList.GetItem(Index);
		if (item == null) return;

		if (string.IsNullOrWhiteSpace(NewText))
		{
			Console.WriteLine("Ошибка: не указан новый текст задачи");
			return;
		}

		_oldText = item.Text;
		item.UpdateText(NewText);
		Console.WriteLine($"Задача обновлена: {item.Text}");
	}

	public override void Unexecute()
	{
		var item = TodoList.GetItem(Index);
		if (item != null && _oldText != null)
		{
			item.UpdateText(_oldText);
			Console.WriteLine($"Отменено обновление задачи: {item.Text}");
		}
	}
}
