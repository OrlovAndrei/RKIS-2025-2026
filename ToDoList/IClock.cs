using System;

namespace ToDoList
{
	public interface IClock
	{
		DateTime Now { get; }
	}
}