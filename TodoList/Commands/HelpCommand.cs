namespace TodoList;

public class HelpCommand : ICommand
{
    public void Execute()
    {
        Console.WriteLine("""
        Доступные команды:
        help — список команд
        profile — выводит данные пользователя
        add "текст" (--multiline/-m) — добавляет задачу
        view (-index/-i, --status/-s, --update-date/-d, --all/-a) — просмотр задач
        done "индекс" — отметить выполненным
        delete "индекс" — удалить задачу
        update "индекс" "текст" — изменить текст
        read "индекс" — показать полную информацию о задаче
        exit — завершить программу
        """);
    }
}