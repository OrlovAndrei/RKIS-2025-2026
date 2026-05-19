using System;

namespace ToDoList
{
	public class SystemClock : IClock
	{
		public DateTime Now => DateTime.Now;
	}
}