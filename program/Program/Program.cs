using Infrastructure;
using Infrastructure.Database;
using Infrastructure.EfRepository;
using Presentation;

namespace Program;

public static class Program
{
    private static readonly TodoContext _context = new();
    private static readonly EfProfileRepository _profileRepository = new(context: _context);
    private static readonly EfTaskStateRepository _taskStateRepository = new(context: _context);
    private static readonly EfTodoTaskRepository _todoTaskRepository = new(context: _context);
	private static readonly DatabaseInitialization _databaseInitialization = new(context: _context);
    private static readonly CurrentUser _currentUser = new();
    public static async Task<int> Main(string[] args)
    {
        await Launch.UpdateRepositories(
            profileRepository: _profileRepository,
		    taskStateRepository: _taskStateRepository,
		    todoTaskRepository: _todoTaskRepository,
            currentUserService: _currentUser
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