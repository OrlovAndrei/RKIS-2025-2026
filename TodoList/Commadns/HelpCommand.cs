namespace TodoList
{
    public class HelpCommand : ICommand
    {
        public void Execute()
        {
            Console.WriteLine("""
            Доступные команды:
            help — список всех доступных команд
            profile — данные пользователя
            add — добавить новую задачу
            read — полный просмотр задачи
            view — список всех задач
            status — изменить статус задачи
            delete — удалить задачу по номеру
            update — изменение текста задачи
            exit — завершить программу

            Флаги для команды 'view':
            -i, --index — показывать индекс задачи
            -s, --status — показывать статус задачи
            -d, --update-date — показывать дату изменения
            -a, --all — показывать все данные

            Многострочные задачи:
            add --multiline или add -m — добавить многострочную задачу
            update --multiline номер или update -m номер — изменить задачу на многострочную

            Статусы задач:
            NotStarted — не начато
            InProgress — в процессе
            Completed — выполнено
            Postponed — отложено
            Failed — провалено
            """);
        }
    }
}