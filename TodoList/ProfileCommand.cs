
namespace TodoList1;

public class ProfileCommand : BaseCommand
{
	public override void Execute()
	{
		Console.WriteLine(Profile.GetInfo());
	}
}
