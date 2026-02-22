using System.Collections.Generic;
using TodoList.commands;
using TodoList.Exceptions;

namespace TodoList;

public static class AppInfo
{
    public static Dictionary<Guid, TodoList> TodosByUser { get; set; } = new Dictionary<Guid, TodoList>();
    public static List<Profile> Profiles { get; set; } = new List<Profile>();
    public static Guid? CurrentProfileId { get; set; }
    public static Profile CurrentProfile => 
        CurrentProfileId.HasValue ? Profiles.FirstOrDefault(p => p.Id == CurrentProfileId) : null;
    public static Stack<ICommand> UndoStack { get; set; } = new Stack<ICommand>();
    public static Stack<ICommand> RedoStack { get; set; } = new Stack<ICommand>();
    
    public static TodoList GetCurrentTodoList()
    {
        if (!CurrentProfileId.HasValue)
            throw new ProfileNotFoundException("Нет активного профиля. Сначала войдите или создайте профиль.");
        
        if (!TodosByUser.ContainsKey(CurrentProfileId.Value))
            TodosByUser[CurrentProfileId.Value] = new TodoList();
        
        return TodosByUser[CurrentProfileId.Value];
    }
}