namespace TodoList
{
    /// <summary>
    /// Интерфейс для команд приложения TodoList.
    /// </summary>
    internal interface ICommand
    {
        /// <summary>
        /// Выполняет команду.
        /// </summary>
        void Execute();
    }
}

