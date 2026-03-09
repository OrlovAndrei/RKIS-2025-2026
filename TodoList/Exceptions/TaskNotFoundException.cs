namespace Todolist.Exceptions
{
    public class TaskNotFoundException : Exception
    {
        public TaskNotFoundException(string message) : base(message) { }
        public TaskNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}