using System;
using TodoList.Interfaces;

namespace TodoList
{
    public class SystemClock : IClock
    {
        public DateTime Now => DateTime.Now;
    }
}
