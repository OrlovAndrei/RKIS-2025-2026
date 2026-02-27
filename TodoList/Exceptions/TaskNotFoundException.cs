using System;
namespace TodoApp.Exceptions
{
    public class TaskNotFoundException : System.Exception
    {
        public TaskNotFoundException(string message) : base(message) { }
    }
}