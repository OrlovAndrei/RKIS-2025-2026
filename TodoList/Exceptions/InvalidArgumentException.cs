using System;
namespace TodoApp.Exceptions
{
    public class InvalidArgumentException : System.Exception
    {
        public InvalidArgumentException(string message) : base(message) { }
    }
}