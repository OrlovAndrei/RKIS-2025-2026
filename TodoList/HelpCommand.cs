using System;

namespace TodoList
{
    public class HelpCommand : ICommand
    {
        public void Execute()
        {
            Console.WriteLine(
            """
            Команды:
            add <текст> — добавить задачу
            add --multiline (-m) — добавить многострочную задачу
            done <номер> — отметить задачу выполненной
            delete <номер> — удалить задачу
            view [-i][-s][-d][-a] — показать задачи (можно комбинировать, напр. -as)
            profile — показать профиль
            help — список команд
            exit — выход
            """);
        }
    }
}
