using TodoList.Interfaces;

namespace TodoList.Infrastructure;

public class Clock : IClock
{
	public DateTime Now()
	{
		return DateTime.Now;
	}
}