using System;
namespace TodoApp.Exception
{
    public class InvalidCommandException : Exception
    {
        public InvalidCommandException(string message) : base(message) { }
    }
}