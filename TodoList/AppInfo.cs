using System.Collections.Generic;
using System;

public static class AppInfo
{
    public static Dictionary<Guid, TodoList> UserTodos { get; set; }
    public static List<Profile> Profiles { get; set; }
    public static Guid? CurrentProfileId { get; set; }
    public static Stack<ICommand> UndoStack { get; set; }
    public static Stack<ICommand> RedoStack { get; set; }

    static AppInfo()
    {
        UserTodos = new Dictionary<Guid, TodoList>();
        Profiles = new List<Profile>();
        UndoStack = new Stack<ICommand>();
        RedoStack = new Stack<ICommand>();
        CurrentProfileId = null;

        Console.WriteLine("AppInfo инициализирован"); 
    }
    public static TodoList CurrentTodoList
    {
        get
        {
            if (CurrentProfileId.HasValue && UserTodos.ContainsKey(CurrentProfileId.Value))
                return UserTodos[CurrentProfileId.Value];
            return null;
        }
    }

    public static Profile CurrentProfile
    {
        get
        {
            if (CurrentProfileId.HasValue && Profiles != null)
                return Profiles.Find(p => p.Id == CurrentProfileId.Value);
            return null;
        }
    }
}