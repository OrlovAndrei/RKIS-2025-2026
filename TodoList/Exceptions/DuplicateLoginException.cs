using System;

namespace TodoList.Exceptions
{
    public class DuplicateLoginException : Exception
    {
        public DuplicateLoginException() : base("Пользователь с таким логином уже существует.") { }

        public DuplicateLoginException(string login) 
            : base($"Пользователь с логином '{login}' уже существует.") { }

        public DuplicateLoginException(string message, Exception innerException) 
            : base(message, innerException) { }
    }
}