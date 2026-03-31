namespace TodoList
{
    public class HelpCommand : ICommand
    {
        public void Execute()
        {
            System.Console.WriteLine("""
            Доступные команды:
            help - показать список команд
            profile - показать данные пользователя
            profile --out (-o) - выйти из текущего профиля
            
            add "текст задачи" - добавить задачу
            add --multiline (-m) - добавить многострочную задачу (ввод построчно, завершите '!end')
            
            view - показать все задачи
            view --index (-i) - показать с номерами
            view --status (-s) - показать с статусом
            view --update-date (-d) - показать с датой последнего изменения
            view --all (-a) - показать все колонки
            
            read <индекс> - показать полную информацию о задаче
            
            search "текст" [--status <статус>] [--case-sensitive] [--regex] - поиск задач
                --status NotStarted, InProgress, Completed, Postponed, Failed
                --case-sensitive - регистрозависимый поиск
                --regex - использовать регулярное выражение для поиска
            
            undo - отменить последнее действие
            redo - повторить отмененное действие
            
            delete <индекс> - удалить задачу
            update <индекс> "новый текст" - обновить текст задачи
            update <индекс> --multiline (-m) - обновить многострочную задачу
            
            status <индекс> <статус> - изменить статус задачи
                Доступные статусы: NotStarted, InProgress, Completed, Postponed, Failed
            
            exit - выйти из программы
            
            Примеры поиска:
              search "купить"                    - поиск задач, содержащих "купить"
              search "срочно" --status InProgress - поиск незавершенных срочных задач
              search "ошибка" --case-sensitive   - регистрозависимый поиск
              search "\d{4}-\d{2}-\d{2}" --regex  - поиск по регулярному выражению
            """);
        }

        public void Unexecute() { }
    }
}