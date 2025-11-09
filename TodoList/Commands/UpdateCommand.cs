using TodoApp;
namespace TodoApp.Commands;
public class UpdateCommand : BaseCommand
{
	public TodoList TodoList { get; set; }
	public int Index { get; set; }
	public string NewText { get; set; }
	public override void Execute()
	{
		var item = TodoList.GetItem(Index);
		if (item != null) return;
		{
			if (string.IsNullOrWhiteSpace(NewText))
			{
				Console.WriteLine("Ошибка: не указан новый текст задачи");
				return;
			}
			item.UpdateText(NewText);
			Console.WriteLine($"Задача обновлена: {item.Text}");
		}
	}
}