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
				case Verb.TaskAdd ta:
					await RunTaskCommands.RunAdd(ta);
					break;
				case Verb.TaskRemove tr:
					await RunTaskCommands.RunRemove(tr);
					break;
				case Verb.TaskEdit te:
					await RunTaskCommands.RunEdit(te);
					break;
				case Verb.TaskSearch ts:
					await RunTaskCommands.RunSearch(ts);
					break;
				case Verb.TaskList tl:
					await RunTaskCommands.RunList(tl);
					break;
				case Verb.ProfileAdd pa:
					await RunProfileCommands.RunAdd(pa);
					break;
				case Verb.ProfileRemove pr:
					await RunProfileCommands.RunRemove(pr);
					break;
				case Verb.ProfileChange pc:
					await RunProfileCommands.RunChange(pc);
					break;
				case Verb.ProfileEdit pe:
					await RunProfileCommands.RunEdit(pe);
					break;
				case Verb.ProfileSearch ps:
					await RunProfileCommands.RunSearch(ps);
					break;
				case Verb.ProfileList pl:
					await RunProfileCommands.RunList(pl);
					break;
				case Verb.Redo r:
					await Launch.CommandManager.Redo();
					break;
				case Verb.Undo u:
					await Launch.CommandManager.Undo();
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
