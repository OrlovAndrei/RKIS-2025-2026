using CommandLine;

namespace ConsoleApp.Parser.Verb;

[Verb(name: "undo", isDefault: false, HelpText = "Откатиться до предыдущих действий.")]
internal class Undo
{
}
