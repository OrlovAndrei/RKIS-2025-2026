namespace Todolist.Exceptions
{
    public class DuplicateLoginException : Exception
    {
        public DuplicateLoginException(string message) : base(message) { }
    }
}