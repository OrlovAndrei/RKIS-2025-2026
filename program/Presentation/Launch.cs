using Presentation.Parser;
using static System.Console;
using Application.Interfaces;
using Domain.Interfaces;

namespace Presentation;

public static class Launch
{
	internal static IProfileRepository? ProfileRepository { get; private set; }
	internal static ITodoTaskRepository? TodoTaskRepository { get; private set; }
	internal static IUserContext? UserContext { get; private set; }
	private static bool _run = true;
	private static bool _isRepositories = false;
	public static async Task UpdateRepositories(
		IProfileRepository profileRepository,
		ITodoTaskRepository todoTaskRepository,
		IUserContext userContextService
	)
	{
		ProfileRepository = profileRepository;
		TodoTaskRepository = todoTaskRepository;
		UserContext = userContextService;
		_isRepositories = true;
	}
	public static async Task<short> RunOnce(string[] args)
	{
		if (!_isRepositories)
		{
			throw new Exception(message: "The repositories have not been updated.");
		}
		try
		{
			Parse.Run(args: args);
			return 0;
		}
		catch (Exception ex)
		{
			Input.WriteToConsole.ProcExcept(ex);
			return 1;
		}
	}
	public static async Task<short> CyclicRun()
	{
		if (!_isRepositories)
		{
			throw new Exception(message: "The repositories have not been updated.");
		}
		try
		{
			int cycles = 0;
			while (_run)
			{
				Write("> ");
				string inputTerminal = ReadLine() ?? "--help";
				string[] args = inputTerminal.Split(
					separator: " ",
					options: StringSplitOptions.TrimEntries |
					StringSplitOptions.RemoveEmptyEntries);
				Parse.Run(args: args);
				cycles++;
			}
			return 0;
		}
		catch (Exception ex)
		{
			Input.WriteToConsole.ProcExcept(ex);
			return 1;
		}
	}
	internal static void Exit()
	{
		_run = false;
	}
}
