namespace TodoList.commands;

public class HelpCommand : ICommand
{
    public void Execute()
    {
        Console.WriteLine("Доступные команды:");
        Console.WriteLine("help                          - показать эту справку");
        Console.WriteLine("add [--multiline|-m] <task>   - добавить задачу (многострочный режим с флагом)");
        Console.WriteLine("view [--index|-i] [--status|-s] [--update-date|-d] [--all|-a]");
        Console.WriteLine("                              - показать задачи с флагами отображения");
        Console.WriteLine("read <idx>                    - прочитать полный текст задачи");
        Console.WriteLine("setstatus <num> <status>      - изменить статус задачи");
        Console.WriteLine("delete <num>                  - удалить задачу");
        Console.WriteLine("update <num> \"<text>\"       - обновить текст задачи");
        Console.WriteLine("undo                          - отменить последнее действие");
        Console.WriteLine("redo                          - повторить отмененное действие");
        Console.WriteLine("profile [--out|-o]            - управление профилем (флаг --out для выхода)");
        Console.WriteLine("exit                          - выйти из программы");
        Console.WriteLine();
        Console.WriteLine("Флаги для команды view:");
        Console.WriteLine("  --index, -i       - показывать индекс задачи");
        Console.WriteLine("  --status, -s      - показывать статус задачи");
        Console.WriteLine("  --update-date, -d - показывать дату изменения");
        Console.WriteLine("  --all, -a         - показывать все данные");
        Console.WriteLine("Комбинации флагов: view -is, view --index --status и т.д.");
        Console.WriteLine();
        Console.WriteLine("Доступные статусы задач:");
        Console.WriteLine("  NotStarted, InProgress, Completed, Postponed, Failed");
        Console.WriteLine();
        Console.WriteLine("Система многопользовательского режима:");
        Console.WriteLine("  - Каждый пользователь имеет свой логин и пароль");
        Console.WriteLine("  - Задачи хранятся отдельно для каждого пользователя");
        Console.WriteLine("  - Для выхода из текущего профиля используйте 'profile --out'");
    }

    public void Unexecute()
    {
        // Команда help не требует отмены
    }
}