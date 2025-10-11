using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;

namespace Task
{
    class CommandsJson
    {
        public Command[]? Commands { get; set; }
    }
    class Command
    {
        public string? Name { get; set; }
        public Option[]? Options { get; set; }
    }
    class Option
    {
        public string? Name { get; set; } = null;
        public string? Long { get; set; } = null;
        public string? Short { get; set; } = null;
    }

    class SearchCommandOnJson
    {
        static CommandsJson? openJsonFile = JsonSerializer.Deserialize<CommandsJson?>
        (FileWriter.StringFromFileInMainFolder("Commands.json"));
        public string commandOut = ConstProgram.StringNull;
        public string[] optionsOut = ConstProgram.StringArrayNull;
        public string nextTextOut = ConstProgram.StringNull;
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
                                                        optionsLine.Append(ConstProgram.SeparRows + option.Name);
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
                                                        optionsLine.Append(ConstProgram.SeparRows + option.Name);
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
                                                            optionsLine.Append(ConstProgram.SeparRows + subOption.Name);
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
                                    if (textLine.ToString() == "")
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
                if (optionsLine.ToString() != "")
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
        public bool SearchOption(string[] options)
        {
            if (optionsOut != ConstProgram.StringArrayNull &&
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
}
