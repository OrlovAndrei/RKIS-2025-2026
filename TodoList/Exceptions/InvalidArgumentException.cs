using System;

public class InvalidArgumentException : Exception
{
    public InvalidArgumentException(string message) : base(message) { }
    
    public InvalidArgumentException(string argumentName, object invalidValue, string reason) 
        : base($"Неверный аргумент '{argumentName}' со значением '{invalidValue}': {reason}") { }
}