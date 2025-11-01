namespace TodoList;

public class CommandParser
{
    public static string[] ParseFlags(string command)
    {
        var parts = command.Split(' ');
        var flags = new List<string>();

        foreach (var part in parts)
        {
            if (part.StartsWith("--"))
            {
                flags.Add(part);
            }
            else if (part.StartsWith("-"))
            {
                for (int i = 1; i < part.Length; i++)
                    flags.Add("-" + part[i]);
            }
        }

        return flags.ToArray();
    }
}