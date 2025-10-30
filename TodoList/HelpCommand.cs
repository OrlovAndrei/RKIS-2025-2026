namespace TodoList
{
    public class HelpCommand : ICommand
    {
        public void Execute()
        {
            Console.WriteLine("Доступные команды:");
            Console.WriteLine("help — список всех доступных команд");
            Console.WriteLine("profile — данные пользователя");
            Console.WriteLine("add — добавить новую задачу");
            Console.WriteLine("read — полный просмотр задачи");
            Console.WriteLine("view — список всех задач");
            Console.WriteLine("done — отметить задачу как выполненную");
            Console.WriteLine("delete — удалить задачу по номеру");
            Console.WriteLine("update — изменение текста задачи");
            Console.WriteLine("exit — завершить программу");
            Console.WriteLine();
            Console.WriteLine("Флаги для команды 'view':");
            Console.WriteLine(" -i, --index — показывать индекс задачи");
            Console.WriteLine(" -s, --status — показывать статус задачи");
            Console.WriteLine(" -d, --update-date — показывать дату изменения");
            Console.WriteLine(" -a, --all — показывать все данные");
        }
    }
}