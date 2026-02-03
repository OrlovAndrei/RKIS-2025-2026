namespace ShevricTodo.Parser;

internal class RunOptions
{
	public async static void Run(object obj)
	{
		switch (obj)
		{
			case Verb.Task t:
				//
				break;
			case Verb.Profile p:
				//
				break;
			case Verb.Redo r:
				//
				break;
			case Verb.Undo u:
				//
				break;
			case Verb.Exit e:
				//
				break;
			case Verb.Run e:
				await Program.Run();
				break;
		}
	}
}
