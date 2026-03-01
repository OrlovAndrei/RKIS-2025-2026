using System;
using System.Collections.Generic;

namespace TodoApp.Commands
{
    public class HelpCommand : BaseCommand
    {
        public override string Name => "help";
        public override string Description => "Показать все команды";

        public List<ICommand> AvailableCommands { get; set; }

        public override bool Execute()
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

            Console.WriteLine("\nКоманды с отменой (undo/redo):");
            Console.WriteLine("add, update, remove, status");
            Console.WriteLine("\nКоманды просмотра (без отмены):");
            Console.WriteLine("view, read, modify, help");
            Console.WriteLine("\nУправление историей:");
            Console.WriteLine("undo - отменить последнее действие");
            Console.WriteLine("redo - повторить отмененное действие");

            return true;
        }

        public override bool Unexecute()
        {
            // Команда help не изменяет состояние
            return true;
        }
    }
}
