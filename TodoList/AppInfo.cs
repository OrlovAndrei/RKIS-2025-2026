using System.Collections.Generic;
using System;

public static class AppInfo
{
    public static Dictionary<Guid, TodoList> UserTodos { get; set; } = new();
    public static List<Profile> Profiles { get; set; } = new();
    public static Guid? CurrentProfileId { get; set; }
    public static Stack<IUndo> UndoStack { get; set; } = new();
    public static Stack<IUndo> RedoStack { get; set; } = new();

    public static TodoList? CurrentTodoList =>
        CurrentProfileId.HasValue && UserTodos.ContainsKey(CurrentProfileId.Value)
            ? UserTodos[CurrentProfileId.Value]
            : null;

    public static Profile? CurrentProfile =>
        CurrentProfileId.HasValue && Profiles != null
            ? Profiles.Find(p => p.Id == CurrentProfileId.Value)
            : null;
}