namespace TodoList.Commands;

public class HelpCommand : ICommand
{
    public void Execute()
    {
        Console.WriteLine(
            """
            Доступные команды:
            help - вывести список команд
            profile - показать данные пользователя
            setprofile - ввести данные пользователя
            add \"текст задачи\" - добавить новую задачу
              -m, --multi — добавить задачу в несколько строк
            status [id] [enum] - изменить статус задачи
            delete [id] - удалить задачу
            update [id] \"новый текст\" - обновить текст задачи
            view - показать задачи в табличном виде
              -a, --all — добавить все поля
              -i, --index — добавить индекс
              -s, --status — добавить статус
              -d, --update-date — добавить дату
            read [id] — вывод задачи
            exit - выйти из программы
            """);
    }
}