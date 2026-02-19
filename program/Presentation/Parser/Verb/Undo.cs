using CommandLine;

namespace Presentation.Parser.Verb;

[Verb(name: "undo", isDefault: false, HelpText = "Откатиться до предыдущих действий.")]
internal class Undo
{
}
