namespace Todolist.Exceptions
{
    public class ProfileNotFoundException : Exception
    {
        public ProfileNotFoundException(string message) : base(message) { }
        public ProfileNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}