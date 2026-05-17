using TodoList.Services;
using TodoList.commands;

namespace TodoList;

public static class AppInfo
{
    public static TodoRepository TodoRepo { get; set; } = new TodoRepository();
    public static ProfileRepository ProfileRepo { get; set; } = new ProfileRepository();

    // Для обратной совместимости (некоторые команды могут ещё использовать TodosByUser)
    public static Dictionary<Guid, TodoList> TodosByUser { get; set; } = new Dictionary<Guid, TodoList>();
    public static List<Profile> Profiles { get; set; } = new List<Profile>();

    public static Guid? CurrentProfileId { get; set; }
    public static Profile CurrentProfile => CurrentProfileId.HasValue ? ProfileRepo.GetById(CurrentProfileId.Value) : null;

    public static Stack<ICommand> UndoStack { get; set; } = new Stack<ICommand>();
    public static Stack<ICommand> RedoStack { get; set; } = new Stack<ICommand>();

    // Метод для получения текущего списка задач (кэш, но можно переделать)
    public static TodoList GetCurrentTodoList()
    {
        if (!CurrentProfileId.HasValue)
            throw new Exceptions.ProfileNotFoundException("Нет активного профиля.");

        if (!TodosByUser.ContainsKey(CurrentProfileId.Value))
            TodosByUser[CurrentProfileId.Value] = new TodoList();

        return TodosByUser[CurrentProfileId.Value];
    }
}