
namespace TodoList1;

public class UpdateCommand : BaseCommand
{
	public int Index { get; set; }
	public string NewText { get; set; }

	public override void Execute()
	{
		var item = todoList.GetItem(Index);
		if (item != null)
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