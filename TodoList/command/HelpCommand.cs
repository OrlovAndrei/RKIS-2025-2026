using System;
using System.Collections.Generic;

namespace TodoApp.Commands
{
    public class HelpCommand : ICommand
    {
        public string Name => "help";
        public string Description => "Показать все команды";

        // Список всех доступных команд
        public List<ICommand> AvailableCommands { get; set; }

        public bool Execute()
        {
            if (AvailableCommands == null || AvailableCommands.Count == 0)
            {
                Console.WriteLine(" Ошибка: список команд не доступен");
                return false;
            }

            Console.WriteLine("\n=== СПРАВКА ПО КОМАНДАМ ===");
            foreach (var command in AvailableCommands)
            {
                Console.WriteLine($"{command.Name,-10} - {command.Description}");
            }

            Console.WriteLine("\nФлаги для команд:");
            Console.WriteLine("add:    -m  - многострочный ввод");
            Console.WriteLine("view:   -i  - показывать номера задач");
            Console.WriteLine("        -s  - показывать статус выполнения");
            Console.WriteLine("        -d  - показывать дату изменения");
            Console.WriteLine("remove: -f  - принудительное удаление");
            Console.WriteLine("\nСтатусы для команды status:");
            Console.WriteLine("notstarted - Не начата");
            Console.WriteLine("inprogress - В процессе");
            Console.WriteLine("completed  - Выполнена");
            Console.WriteLine("postponed  - Отложена");
            Console.WriteLine("failed     - Провалена");

            return true;
        }
    }
}
