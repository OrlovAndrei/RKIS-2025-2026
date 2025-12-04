using YamlDotNet.Serialization;

namespace TodoList;

public class Command
{
    public string? Name { get; set; }
    public Option[]? Options { get; set; }
    public class Option
    {
        public string? Name { get; set; } = null;
        public string? Long { get; set; } = null;
        public string? Short { get; set; } = null;
    }
}
public class SearchCommand
{
    private static IDeserializer deserializer = new DeserializerBuilder()
        .Build();
    private static string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DataTypeAndCommands", "Commands.yaml");
    public static List<Command> commands = deserializer.Deserialize<List<Command>>(File.OpenText(fullPath));
    public string? Command { get; private set; }
	public List<string>? Options { get; private set; } = [];
	public string? Argument { get; private set; }
	private Command? ActiveCommand { get; set; }
    public SearchCommand(string[] commandLine)
	{
		List<string> optionsList = new();
		List<string> argumentList = new();
		foreach (var command in commands!)
		{
			if (command.Name == commandLine[0])
			{
				ActiveCommand = command;
				Command = ActiveCommand.Name;
				break;
			}
		}
		bool isOptions = true;
		if (ActiveCommand is not null)
		{
			foreach (var pathText in commandLine[1..])
			{
				bool inNotOption = true;
				if (isOptions)
				{
					foreach (var option in ActiveCommand!.Options!)
					{
						if (pathText.Length >= 3 && pathText[0..2] == "--" && pathText == option.Long)
						{
							AddInListNoRepetitions(ref optionsList, option.Name!);
							inNotOption = false;
						}
						else if (pathText.Length == 2 && pathText[0] == '-' && pathText == option.Short)
						{
							AddInListNoRepetitions(ref optionsList, option.Name!);
							inNotOption = false;
						}
						else if (pathText.Length > 2 && pathText[0] == '-' && pathText[1] != '-')
						{
							for (int i = 1; i < pathText.Length; i++)// начинаем с 1 что бы не искать знак -
							{
								foreach (var subOption in ActiveCommand!.Options!)
								{
									if (subOption.Short != null &&
									pathText[i] == char.Parse(subOption.Short[1..subOption.Short.Length]))
									{
										AddInListNoRepetitions(ref optionsList, subOption.Name!);
										inNotOption = false;
									}
								}
							}
						}
					}
					if (inNotOption)
					{
						argumentList.Add(pathText);
						isOptions = false;
					}
					if (optionsList.Count != 0)
					{
						Options = optionsList;
					}
					Argument = string.Join(' ', argumentList);
				}
			}
		}
	}
	private void AddInListNoRepetitions(ref List<string> list, string input)
	{
		if (!list.Contains(input))
		{
			list.Add(input);
		}
	}
}
