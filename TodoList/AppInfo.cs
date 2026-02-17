using System.Collections.Generic;

namespace Todolist
{
    public static class AppInfo
    {
        public static Dictionary<Guid, Todolist> UserTodos { get; set; } = new Dictionary<Guid, Todolist>();
        public static List<Profile> Profiles { get; set; } = new List<Profile>();
        public static Guid? CurrentProfileId { get; set; }
        public static Stack<ICommand> UndoStack { get; set; } = new Stack<ICommand>();
        public static Stack<ICommand> RedoStack { get; set; } = new Stack<ICommand>();

        public static Todolist GetCurrentTodos()
        {
            if (CurrentProfileId.HasValue && UserTodos.ContainsKey(CurrentProfileId.Value))
            {
                return UserTodos[CurrentProfileId.Value];
            }
            return new Todolist();
        }

        public static Profile GetCurrentProfile()
        {
            if (CurrentProfileId.HasValue)
            {
                return Profiles.Find(p => p.Id == CurrentProfileId.Value);
            }
            return null;
        }

        public static void ClearUndoRedoStacks()
        {
            UndoStack.Clear();
            RedoStack.Clear();
        }
    }
}