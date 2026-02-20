using System;

namespace TodoList.Exceptions
{
	public class InvalidCommandException : Exception
	{
		public InvalidCommandException(string message) : base(message) { }
	}
	public class InvalidArgumentException : Exception
	{
		public InvalidArgumentException(string message) : base(message) { }
	}
	public class TaskNotFoundException : Exception
	{
		public TaskNotFoundException(string message) : base(message) { }
	}
	public class ProfileNotFoundException : Exception
	{
		public ProfileNotFoundException(string message) : base(message) { }
	}
	public class AuthenticationException : Exception
	{
		public AuthenticationException(string message) : base(message) { }
	}
	public class DuplicateLoginException : Exception
	{
		public DuplicateLoginException(string message) : base(message) { }
	}
}