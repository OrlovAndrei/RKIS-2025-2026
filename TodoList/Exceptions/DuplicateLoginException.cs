namespace Todolist.Exceptions
{
    public class DuplicateLoginException : Exception
    {
        public DuplicateLoginException(string message) : base(message) { }
        public DuplicateLoginException(string message, Exception innerException) : base(message, innerException) { }
    }
}