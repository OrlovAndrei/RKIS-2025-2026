using System;

namespace TodoList.Exceptions
{
    public class ProfileNotFoundException : Exception
    {
        public ProfileNotFoundException() : base("Профиль не найден.") { }

        public ProfileNotFoundException(string login) 
            : base($"Пользователь с логином '{login}' не найден.") { }

        public ProfileNotFoundException(string message, Exception innerException) 
            : base(message, innerException) { }
    }
}