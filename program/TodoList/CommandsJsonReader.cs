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
public class SearchCommandOnJson
{
	private static CommandsJson? openJsonFile = JsonSerializer.Deserialize<CommandsJson?>
	(OpenFile.StringFromFileInMainFolder("Commands.json"));
	public string commandOut = "";
	public string[] optionsOut = Const.StringArrayNull;
	public string nextTextOut = "";
	public SearchCommandOnJson(string[] text)
	{
		StringBuilder optionsLine = new();
		StringBuilder textLine = new();
		if (openJsonFile != null &&
		openJsonFile.Commands != null)
		{
			foreach (var command in openJsonFile.Commands)
			{
				if (command.Name == text[0])
				{
					commandOut = command.Name;
					if (command.Options != null)
					{
						Range withoutFirstString = 1..text.Length;
						bool isOptions = true;
						foreach (var pathText in text[withoutFirstString])
						{
							bool inNotOption = true;
							if (isOptions)
							{
								foreach (var option in command.Options)
								{
									if (option.Name != null)
									{
										if (pathText.Length >= 3 && pathText[0..2] == "--")
										{
											if (!optionsLine.ToString().Contains(option.Name) && pathText == option.Long)
											{
												if (optionsLine.Length == 0)
												{
													optionsLine.Append(option.Name);
												}
												else
												{
													optionsLine.Append(Const.SeparRows + option.Name);
												}
												inNotOption = false;
											}
										}
										else if (pathText.Length == 2 && pathText[0] == '-')
										{
											if (!optionsLine.ToString().Contains(option.Name) && pathText == option.Short)
											{
												if (optionsLine.Length == 0)
												{
													optionsLine.Append(option.Name);
												}
												else
												{
													optionsLine.Append(Const.SeparRows + option.Name);
												}
												inNotOption = false;
											}
										}
										else if (pathText.Length > 2 && pathText[0] == '-')
										{
											foreach (var subOption in command.Options)
											{
												if (subOption.Name != null && subOption.Short != null &&
												pathText[1..pathText.Length].Contains(subOption.Short[1..subOption.Short.Length]) &&
												!optionsLine.ToString().Contains(subOption.Name))
												{
													if (optionsLine.Length == 0)
													{
														optionsLine.Append(subOption.Name);
													}
													else
													{
														optionsLine.Append(Const.SeparRows + subOption.Name);
													}
													inNotOption = false;
												}
											}
										}
									}
								}
							}
							if (inNotOption)
							{
								if (textLine.ToString().Length == 0)
								{
									isOptions = false;
									textLine.Append(pathText);
								}
								else { textLine.Append(" " + pathText); }
							}
						}
					}
					break;
				}
			}
			if (optionsLine.ToString().Length != 0)
			{
				optionsOut = optionsLine.ToString().Split("|");
			}
			nextTextOut = textLine.ToString();
			// System.Console.WriteLine("com: " + commandOut); //test
			// System.Console.WriteLine("opt:"); //test
			// if (optionsOut != null) //test
			// { //test
			//     foreach (var option in optionsOut) //test
			//     { //test
			//         System.Console.WriteLine("\t" + option); //test
			//     } //test
			// } //test
			// System.Console.WriteLine("text: " + nextTextOut); //test
		}
	}
	public bool SearchOption(params string[] options)
	{
		if (optionsOut != Const.StringArrayNull &&
		optionsOut != null)
		{
			int count = 0;
			int length = options.Length;
			if (optionsOut.Length == length)
			{
				foreach (var option in options)
				{
					// System.Console.WriteLine($"\t\n{option}"); //test
					if (optionsOut.Contains(option))
					{
						++count;
					}
					else return false;
				}
				if (count == length)
				{
					// System.Console.WriteLine("\tif (count == length)"); //test
					// System.Console.WriteLine($"\t{count} == {length}\n"); //test
					return true;
				}
			}
		}
		return false;
	}
}
