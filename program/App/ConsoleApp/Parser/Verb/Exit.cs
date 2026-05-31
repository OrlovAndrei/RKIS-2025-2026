using CommandLine;

namespace ConsoleApp.Parser.Verb;

[Verb(name: "exit", isDefault: false, HelpText = "Выход из программы.")]
internal class Exit { }
