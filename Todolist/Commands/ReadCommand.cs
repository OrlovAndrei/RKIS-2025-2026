using System;
namespace Todolist.Commands
{
    internal class ReadCommand : ICommand
    {
        public int Index { get; set; }

        public ReadCommand(int index)
        {
            Index = index;
        }

        public void Execute()
        {
            try
            {
                AppInfo.Todos.Read(Index);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        public void Unexecute()
        {
            // команда только читает данные, откат не нужен
        }
    }
}

