using Presentation.Parser;
using static System.Console;
using Infrastructure.Database;
using Infrastructure;
using Infrastructure.EfRepository;
using Application.UseCase.ProfileUseCases;
using Application.Interfaces;

namespace Presentation;

internal class Program
{
	private static bool _run = true;
	private static readonly TodoContext _context = new();
	private static readonly PasswordHashed _passwordHashed = new();
	private static readonly EfProfileRepository _profileRepository = new(context: _context);
	private static readonly EfTaskStateRepository _taskStateRepository = new(context: _context);
	private static readonly EfTodoTaskRepository _todoTaskRepository = new(context: _context);
	public static readonly ProfileUseCase profileUseCase = new(repository: _profileRepository, passwordHashed: _passwordHashed);
	public static ICurrentUserService currentUser = new CurrentUser();
	public static void Main(string[] args)
	{
		try
		{
			for (int i = 0; i < 10000; i++)
			{
				DatabaseInitialization databaseInitialization = new(context: _context);
				databaseInitialization.Initialize();
				currentUser = profileUseCase.CurrentUserServiceTest(profileId: default, password: "1234567890", currentUserService: new CurrentUser());
			}
			Parse.Run(args: args);
		}
		catch (Exception ex)
		{
			Input.WriteToConsole.ProcExcept(ex);
		}
	}
	public static async Task Run()
	{
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
		}
		catch (Exception ex)
		{
			Input.WriteToConsole.ProcExcept(ex);
		}
	}
	public static void Exit()
	{
		_run = false;
	}
}
