using System;

namespace Todolist.Exceptions
{
    public class StorageException : Exception
    {
        public StorageException(string message, Exception? innerException = null)
            : base(message, innerException)
        {
        }
    }
}
