namespace TodoList.commands
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
            status <номер> <status> — изменить статус
            delete <номер> — удалить задачу
            update <номер> <текст> — удалить задачу
            view [-i][-s][-d][-a] — показать задачи (можно комбинировать, напр. -as)
            read — показать информацию о задаче
            profile — показать профиль
            help — список команд
            undo - отменить последнее действие
            redo - повторить отмененное действие
            exit — выход
            """);
        }

        public void Unexecute()
        {
            
        }
    }
}
