using CommandLine;

namespace ShevricTodo.Parser.Verb;

[Verb(name: "redo", isDefault: false, HelpText = "Восстановить предыдущее действие.")]
internal class Redo
{
}
