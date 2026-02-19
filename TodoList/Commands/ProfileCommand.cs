namespace TodoList
{
    public class ProfileCommand : ICommand
    {
        private readonly bool _logout;

        public ProfileCommand(bool logout = false)
        {
            _logout = logout;
        }

        public void Execute()
        {
            if (_logout)
            {
                Console.WriteLine("Выход из текущего профиля...");
                
                AppInfo.UndoStack.Clear();
                AppInfo.RedoStack.Clear();
                AppInfo.CurrentProfileId = null;
                
                AppInfo.ShouldLogout = true;
                
                Console.WriteLine("Текущий профиль сброшен. Стеки отмены/повтора очищены.");
            }
            else
            {
                if (AppInfo.CurrentProfile != null)
                {
                    Console.WriteLine(AppInfo.CurrentProfile.GetInfo());
                }
                else
                {
                    Console.WriteLine("Нет активного профиля.");
                }
            }
        }

        public void Unexecute() { }
    }
}