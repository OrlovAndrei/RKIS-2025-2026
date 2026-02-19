using CommandLine;

namespace Presentation.Parser.Verb;

[Verb(name: "exit", isDefault: false, HelpText = "Выход из программы.")]
internal class Exit { }
