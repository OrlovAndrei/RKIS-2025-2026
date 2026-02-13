namespace ShevricTodo;

internal static class ProgramConst
{
	public static readonly string AppName = nameof(ShevricTodo);
	public static readonly string IfValueIsNullOrWhiteSpace = "N/A";
	public static string NotAvailable(this string? value)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			return IfValueIsNullOrWhiteSpace;
		}
		return value;
	}
}
