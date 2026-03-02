using System;

public class TaskNotFoundException : Exception
{
    public int TaskNumber { get; }

    public TaskNotFoundException(string message) : base(message) { }
    
    public TaskNotFoundException(int taskNumber) 
        : base($"Задача с номером {taskNumber} не существует.")
    {
        TaskNumber = taskNumber;
    }
}