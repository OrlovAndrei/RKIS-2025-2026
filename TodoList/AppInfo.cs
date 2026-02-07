using System.Collections.Generic;

namespace TodoList
{
    public static class AppInfo
    {
        public static List<Profile> Profiles { get; set; } = new List<Profile>();
        public static Dictionary<Guid, TodoList> TodosByUser { get; set; } = new Dictionary<Guid, TodoList>();
        public static Guid? CurrentProfileId { get; set; }
        public static Stack<ICommand> UndoStack { get; set; } = new Stack<ICommand>();
        public static Stack<ICommand> RedoStack { get; set; } = new Stack<ICommand>();
        public static bool ShouldLogout { get; set; }

        public static TodoList CurrentTodos
        {
            get
            {
                if (CurrentProfileId.HasValue && TodosByUser.ContainsKey(CurrentProfileId.Value))
                    return TodosByUser[CurrentProfileId.Value];
                return null;
            }
        }

        public static Profile CurrentProfile
        {
            get
            {
                if (CurrentProfileId.HasValue)
                    return Profiles.Find(p => p.Id == CurrentProfileId.Value);
                return null;
            }
        }
    }
}