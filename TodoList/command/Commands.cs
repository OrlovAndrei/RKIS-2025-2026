using System;

namespace TodoApp
{
    public interface ICommand
    {
        string Name { get; }
        string Description { get; }
        bool Execute();
        bool Unexecute(); // Метод для отмены действия
    }
}
