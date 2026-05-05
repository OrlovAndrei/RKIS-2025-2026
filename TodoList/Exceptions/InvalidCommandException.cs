using System;
namespace TodoApp.Exceptions
{
    public class InvalidCommandException : System.Exception
    {
        public InvalidCommandException(string message) : base(message) { }
    }
}