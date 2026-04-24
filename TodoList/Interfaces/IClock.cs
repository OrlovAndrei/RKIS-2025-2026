using System;

namespace TodoList.Interfaces
{
    public interface IClock
    {
        DateTime Now { get; }
    }
}