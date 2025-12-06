using System;

namespace Todolist
{
    public class HelpCommand : ICommand
    {
        public void Execute()
        {
            Console.WriteLine("""
            Доступные команды:
            help    - выводит список всех доступных команд с кратким описанием
            profile - выводит данные пользователя (флаги -o --out - выход из профиля)
            add     - добавляет новую задачу (флаги -m --multiline - многострочный режим)
            read    - полный просмотр задачи
            view    - выводит все задачи из массива
            status  - изменяет статус задачи
            delete  - удаляет задачу по индексу
            update  - обновляет текст задачи
            undo    - отменяет последнее действие
            redo    - повторяет отмененное действие
            exit    - завершает программу

            Флаги для команды 'view':
            -i, --index       - показывать индекс задачи
            -s, --status      - показывать статус задачи
            -d, --update-date - показывать дату изменения
            -a, --all         - показывать все данные

            Cтатусы задач для комманды 'status':
            NotStarted, InProgress, Completed, Postponed, Failed
            """);
        }

        public void Unexecute()
        {
            throw new NotImplementedException("Команда help не поддерживает отмену");
        }
    }
}