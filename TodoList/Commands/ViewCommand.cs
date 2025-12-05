namespace TodoList
{
    public class ViewCommand : ICommand
    {
        private readonly bool _showIndex;
        private readonly bool _showStatus;
        private readonly bool _showDate;

        public ViewCommand(bool showIndex, bool showStatus, bool showDate)
        {
            _showIndex = showIndex;
            _showStatus = showStatus;
            _showDate = showDate;
        }

        public void Execute()
        {
            if (AppInfo.CurrentTodos == null)
            {
                Console.WriteLine("Ошибка: нет активного профиля.");
                return;
            }
            AppInfo.CurrentTodos.View(_showIndex, _showStatus, _showDate);
        }

        public void Unexecute() { }
    }
}