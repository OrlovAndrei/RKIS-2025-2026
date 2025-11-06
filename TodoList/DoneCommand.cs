namespace TodoList1.Commands;
public class DoneCommand : BaseCommand
{
	public TodoList TodoList { get; set; }
	public int Index { get; set; }

	public override void Execute()
	{
		var item = TodoList.GetItem(Index);
		if (item != null)
			Console.WriteLine(item.MarkDone());
	}
}