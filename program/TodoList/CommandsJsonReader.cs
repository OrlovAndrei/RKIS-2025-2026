using System.Text;
using System.Text.Json;
namespace Task;

internal class CommandsJson
{
	public Command[]? Commands { get; set; }
}

internal class Command
{
	public string? Name { get; set; }
	public Option[]? Options { get; set; }
}

internal class Option
{
	public string? Name { get; set; } = null;
	public string? Long { get; set; } = null;
	public string? Short { get; set; } = null;
}
public class SearchCommand
{
	private static CommandsJson? openJsonFile = JsonSerializer.Deserialize<CommandsJson?>
	(OpenFile.StringFromFileInMainFolder("Commands.json"));
	public string? Command { get; private set; }
	public List<string>? Options { get; private set; }
	public string? Argument { get; private set; }
	private Command? ActiveCommand { get; set; }
	public SearchCommand(string[] commandLine)
	{
		List<string> optionsList = new();
		StringBuilder argumentLine = new();
		foreach (var command in openJsonFile!.Commands!)
		{
			if (command.Name == commandLine[0])
			{
				ActiveCommand = command;
				Command = ActiveCommand.Name;
				break;
			}
		}
		bool isOptions = true;
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
					else if (pathText.Length > 2 && pathText[0] == '-')
					{
						foreach (var subOption in ActiveCommand!.Options!)
						{
							if (subOption.Short != null &&
							pathText[1..pathText.Length].Contains(subOption.Short[1..subOption.Short.Length]))
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
				if (argumentLine.ToString().Length == 0)
				{
					isOptions = false;
					argumentLine.Append(pathText);
				}
				else { argumentLine.Append(" " + pathText); }
			}
		}
		if (optionsList.Count != 0)
		{
			Options = optionsList;
		}
		Argument = argumentLine.ToString();
	}
	private void AddInListNoRepetitions(ref List<string> list, string input)
	{
		if (!list.Contains(input))
		{
			list.Add(input);
		}
	}
}
