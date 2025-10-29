
namespace TodoList1;

public class DeleteCommand : BaseCommand
{
	public int Index { get; set; }

	public override void Execute()
	{
		var item = todoList.GetItem(Index);
		if (item != null)
		{
			todoList.Delete(Index);
		}
	}
}