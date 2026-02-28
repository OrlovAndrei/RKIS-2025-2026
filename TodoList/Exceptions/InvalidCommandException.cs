using System;

public class InvalidCommandException : Exception
{
    public InvalidCommandException(string message) : base(message) { }
    
    public InvalidCommandException(string command, string reason) 
        : base($"Неизвестная команда '{command}': {reason}") { }
}