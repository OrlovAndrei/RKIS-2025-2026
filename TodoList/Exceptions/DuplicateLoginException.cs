using System;
namespace TodoApp.Exceptions
{
    public class DuplicateLoginException : System.Exception
    {
        public DuplicateLoginException(string message) : base(message) { }
    }
}