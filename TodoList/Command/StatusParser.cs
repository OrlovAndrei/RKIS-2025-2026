using System;

public static class StatusParser
{
    public static TodoStatus? ParseStatus(string statusStr)
    {
        return statusStr.ToLower() switch
        {
            "notstarted" => TodoStatus.NotStarted,
            "inprogress" => TodoStatus.InProgress,
            "completed" => TodoStatus.Completed,
            "postponed" => TodoStatus.Postponed,
            "failed" => TodoStatus.Failed,
            _ => null
        };
    }
    
    public static TodoStatus ParseStatusWithDefault(string statusStr, TodoStatus defaultValue = TodoStatus.NotStarted)
    {
        return statusStr.ToLower() switch
        {
            "notstarted" => TodoStatus.NotStarted,
            "inprogress" => TodoStatus.InProgress,
            "completed" => TodoStatus.Completed,
            "postponed" => TodoStatus.Postponed,
            "failed" => TodoStatus.Failed,
            _ => defaultValue
        };
    }
}