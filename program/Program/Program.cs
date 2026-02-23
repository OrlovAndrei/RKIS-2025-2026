using Infrastructure;
using Infrastructure.Database;
using Infrastructure.EfRepository;
using Infrastructure.Encryption;
using Presentation;

namespace Program;

public static class Program
{
	private static readonly TodoContext _context = new();
	private static readonly EfProfileRepository _profileRepository = new(context: _context);
	private static readonly EfTodoTaskRepository _todoTaskRepository = new(context: _context);
	private static readonly DatabaseInitialization _databaseInitialization = new(context: _context);
	private static readonly PasswordHasher _passwordHasher = new();
	private static readonly UserContext _userContext = new();
	public static async Task<int> Main(string[] args)
	{
		await Launch.UpdateRepositories(
			profileRepository: _profileRepository,
			todoTaskRepository: _todoTaskRepository,
			userContextService: _userContext,
			passwordHasher: _passwordHasher
		);
		await _databaseInitialization.InitializeAsync();
		if (args.Length == 0)
		{
			return await Launch.CyclicRun();
		}
		else
		{
			return await Launch.RunOnce(args: args);
		}
	}
}