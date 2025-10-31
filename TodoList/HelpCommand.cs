using System;

namespace TodoList
{
    public class HelpCommand : ICommand
    {
        public void Execute()
        {
            Console.WriteLine("\nКоманды:");
            Console.WriteLine(" add <текст> — добавить задачу");
            Console.WriteLine(" add --multiline (-m) — добавить многострочную задачу");
            Console.WriteLine(" done <номер> — отметить задачу выполненной");
            Console.WriteLine(" delete <номер> — удалить задачу");
            Console.WriteLine(" view [-i][-s][-d][-a] — показать задачи (можно комбинировать, напр. -as)");
            Console.WriteLine(" profile — показать профиль");
            Console.WriteLine(" help — список команд");
            Console.WriteLine(" exit — выход");
        }
    }

    public class ExitCommand : ICommand
    {
        public void Execute()
        {
            Console.WriteLine("Выход из программы.");
            Environment.Exit(0);
        }
    }
}
