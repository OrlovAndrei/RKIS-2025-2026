using System;

namespace TodoList.Exceptions
{
    public class TaskNotFoundException : Exception
    {
        public TaskNotFoundException() : base("Задача не найдена.") { }

        public TaskNotFoundException(int index) 
            : base($"Задача с номером {index} не существует.") { }

        public TaskNotFoundException(string message) : base(message) { }

        public TaskNotFoundException(string message, Exception innerException) 
            : base(message, innerException) { }
    }
}