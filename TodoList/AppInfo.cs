using System;
using System.Collections.Generic;
using Todolist.Models;
using Todolist.Services;

namespace Todolist
{
    public static class AppInfo
    {
        public static Dictionary<Guid, TodoListService> UserTodos { get; set; } = new();
        public static List<Profile> Profiles { get; set; } = new();
        public static Guid? CurrentProfileId { get; set; }
        public static Stack<ICommand> UndoStack { get; set; } = new();
        public static Stack<ICommand> RedoStack { get; set; } = new();

        public static ProfileRepository ProfileRepository { get; set; } = new();
        public static TodoRepository TodoRepository { get; set; } = new();

        // Оставлено для команды sync (работа с сервером)
        public static IDataStorage? DataStorage { get; set; }

        public static TodoListService GetCurrentTodos()
        {
            if (CurrentProfileId.HasValue && UserTodos.ContainsKey(CurrentProfileId.Value))
                return UserTodos[CurrentProfileId.Value];

            var service = new TodoListService(CurrentProfileId ?? Guid.Empty);
            if (CurrentProfileId.HasValue)
                UserTodos[CurrentProfileId.Value] = service;
            return service;
        }

        public static Profile? GetCurrentProfile()
        {
            if (CurrentProfileId.HasValue)
                return Profiles.Find(p => p.Id == CurrentProfileId.Value);
            return null;
        }

        public static void ClearUndoRedoStacks()
        {
            UndoStack.Clear();
            RedoStack.Clear();
        }
    }
}