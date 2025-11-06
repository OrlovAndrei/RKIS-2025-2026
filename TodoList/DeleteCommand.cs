
namespace TodoList1.Commands;

public class DeleteCommand : BaseCommand
{
	public TodoList TodoList { get; set; }
	public int Index { get; set; }

	public override void Execute()
	{
		var item = TodoList.GetItem(Index);
		if (item != null)
			TodoList.Delete(Index);
	}
}
