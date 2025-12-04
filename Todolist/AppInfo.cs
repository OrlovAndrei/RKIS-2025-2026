using System.Collections.Generic;
using Todolist.Commands;

static class AppInfo
{
    // Словарь списков задач по Id профиля
    public static Dictionary<Guid, TodoList> TodosByProfile { get; } = new Dictionary<Guid, TodoList>();

    // Текущий список задач (для активного профиля)
    private static TodoList _currentTodos = new TodoList();
    public static TodoList Todos
    {
        get => _currentTodos;
        set
        {
            _currentTodos = value ?? new TodoList();
            if (CurrentProfileId != Guid.Empty)
            {
                TodosByProfile[CurrentProfileId] = _currentTodos;
            }
        }
    }

    // Список всех профилей, загруженных из profile.csv
    public static List<Profile> Profiles { get; set; } = new List<Profile>();

    // Id текущего профиля
    public static Guid CurrentProfileId { get; set; }

    // Текущий профиль (вычисляемое свойство по Id)
    public static Profile CurrentProfile
    {
        get
        {
            return Profiles.Find(p => p.Id == CurrentProfileId)
                   ?? throw new InvalidOperationException("Текущий профиль не найден.");
        }
        set
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            // Если такого профиля ещё нет в списке — добавим
            var existing = Profiles.Find(p => p.Id == value.Id);
            if (existing == null)
            {
                Profiles.Add(value);
            }

            CurrentProfileId = value.Id;

            // При смене профиля подхватываем его список задач из словаря (или создаём новый)
            if (!TodosByProfile.TryGetValue(CurrentProfileId, out var list))
            {
                list = new TodoList();
                TodosByProfile[CurrentProfileId] = list;
            }
            _currentTodos = list;
        }
    }

    public static Stack<ICommand> UndoStack { get; set; } = new Stack<ICommand>();
    public static Stack<ICommand> RedoStack { get; set; } = new Stack<ICommand>();
}

