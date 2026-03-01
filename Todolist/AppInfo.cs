using System;
using System.Collections.Generic;
using Todolist.Commands;
using Todolist.Exceptions;

static class AppInfo
{
    public static Dictionary<Guid, TodoList> TodosByProfile { get; } = new Dictionary<Guid, TodoList>();

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

    public static List<Profile> Profiles { get; set; } = new List<Profile>();

    public static Guid CurrentProfileId { get; set; }

    public static Profile CurrentProfile
    {
        get
        {
            if (CurrentProfileId == Guid.Empty)
                throw new AuthenticationException("Необходимо войти в профиль для работы с задачами.");
            return Profiles.Find(p => p.Id == CurrentProfileId)
                   ?? throw new ProfileNotFoundException("Текущий профиль не найден или не выбран.");
        }
        set
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            var existing = Profiles.Find(p => p.Id == value.Id);
            if (existing == null)
            {
                Profiles.Add(value);
            }

            CurrentProfileId = value.Id;

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

