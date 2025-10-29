
namespace TodoList1;

public class DoneCommand : BaseCommand
{
	public int Index { get; set; }

	public override void Execute()
	{
		var item = todoList.GetItem(Index);
		if (item != null)
		{
			Console.WriteLine(item.MarkDone());
		}
	}
}