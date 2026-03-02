using Infrastructure;
using Infrastructure.Database;
using Infrastructure.EfRepository;
using Infrastructure.Encryption;
using ConsoleApp;

namespace Program;
/// <summary>
/// Главный класс программы. Содержит точку входа и инициализацию зависимостей.
/// </summary>
public static class Program
{
	/// <summary>
	/// Контекст базы данных задач.
	/// </summary>
	private static readonly TodoContext _context = new();
	/// <summary>
	/// Репозиторий профилей пользователей.
	/// </summary>
	private static readonly EfProfileRepository _profileRepository = new(context: _context);
	/// <summary>
	/// Репозиторий задач.
	/// </summary>
	private static readonly EfTodoTaskRepository _todoTaskRepository = new(context: _context);
	/// <summary>
	/// Инициализация базы данных.
	/// </summary>
	private static readonly DatabaseInitialization _databaseInitialization = new(context: _context);
	/// <summary>
	/// Хеширование паролей.
	/// </summary>
	private static readonly PasswordHasher _passwordHasher = new();
	/// <summary>
	/// Контекст текущего пользователя.
	/// </summary>
	private static readonly UserContext _userContext = new();
	private static readonly CommandManager _commandManager = new();
	/// <summary>
	/// Точка входа в программу.
	/// </summary>
	/// <param name="args">Аргументы командной строки.</param>
	/// <returns>Код завершения программы.</returns>
	public static async Task<int> Main(string[] args)
	{
		// Обновление репозиториев и сервисов
		await Launch.UpdateRepositories(
			profileRepository: _profileRepository,
			todoTaskRepository: _todoTaskRepository,
			userContextService: _userContext,
			passwordHasher: _passwordHasher,
			commandManager: _commandManager
		);
		// Инициализация базы данных
		await _databaseInitialization.InitializeAsync();
		// Запуск программы в циклическом или однократном режиме
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