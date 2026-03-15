namespace TodoApp.Exceptions
{
	public class LoadCommandException : Exception
	{
		public LoadCommandException(string message) : base(message) { }
	}
}