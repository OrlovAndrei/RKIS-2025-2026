namespace TodoApp.Exceptions
{
	public class DataCorruptedException : StorageException
	{
		public DataCorruptedException(string message) : base(message) { }
		public DataCorruptedException(string message, Exception innerException) : base(message, innerException) { }
	}
}