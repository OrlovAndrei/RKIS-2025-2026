namespace TodoApp.Exceptions
{
	public class DecryptionException : StorageException
	{
		public DecryptionException(string message) : base(message) { }
		public DecryptionException(string message, Exception innerException) : base(message, innerException) { }
	}
}