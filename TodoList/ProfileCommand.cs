namespace TodoList1.Commands;
public class ProfileCommand : BaseCommand
{
	public Profile Profile { get; set; }
	public override void Execute()
	{
		Console.WriteLine(Profile.GetInfo());
	}
}
