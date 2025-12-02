using System;

namespace TodoList
{
    /// <summary>
    /// Команда вывода справки по доступным командам.
    /// </summary>
    internal class HelpCommand : ICommand
    {
        public void Execute()
        {
            Console.WriteLine("Доступные команды:");
            Console.WriteLine("help                   — список доступных команд");
            Console.WriteLine("profile                — вывод данных пользователя");
            Console.WriteLine("add \"текст\"          — добавить новую задачу");
            Console.WriteLine("add --multiline        — добавить задачу в многострочном режиме");
            Console.WriteLine("view                   — показать все задачи (таблица)");
            Console.WriteLine("view --no-index        — показать задачи без индексов");
            Console.WriteLine("view --no-done         — показать задачи без статуса");
            Console.WriteLine("view --no-date         — показать задачи без даты");
            Console.WriteLine("status <idx> <status>  — изменить статус задачи (notstarted, inprogress, completed, postponed, failed)");
            Console.WriteLine("delete <idx>           — удалить задачу");
            Console.WriteLine("update <idx> \"текст\"   — обновить текст задачи");
            Console.WriteLine("read <idx>             — показать полную информацию о задаче");
            Console.WriteLine("undo                   — отменить последнее действие");
            Console.WriteLine("redo                   — повторить последнее отменённое действие");
            Console.WriteLine("exit                   — выход из программы");
        }
    }
}

