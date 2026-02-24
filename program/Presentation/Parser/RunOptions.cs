using Presentation.Output.Implementation;

namespace Presentation.Parser;

internal static class RunOptions
{
	public async static void Run(object obj)
	{
		try
		{
			switch (obj)
			{
				case Verb.Task t:
					await RunTaskCommands.Run(t);
					break;
				case Verb.Profile p:
					await RunProfileCommands.Run(p);
					break;
				case Verb.Redo r:
					// TODO: Implement redo via command history/use case
					break;
				case Verb.Undo u:
					// TODO: Implement undo via command history/use case
					break;
				case Verb.Exit e:
					Launch.Exit();
					break;
				default:
					throw new Exception("Неизвестная команда");
			}
		}
		catch (Exception ex)
		{
			WriteToConsole.ProcExcept(ex);
		}
	}
}
