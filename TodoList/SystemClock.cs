namespace Todolist
{
    public class SystemClock : IClock
    {
        public DateTime Now => DateTime.Now;
    }
}