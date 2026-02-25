using CommandLine;

namespace Presentation.Parser.Verb;

[Verb(name: "profile-add", aliases: ["pa"])]
internal class ProfileAdd
{
	[Option(longName: "first-name", shortName: 'f', Required = true)]
	public string? FirstName { get; set; } = string.Empty;

	[Option(longName: "last-name", shortName: 'l', Required = true)]
	public string? LastName { get; set; } = string.Empty;

	[Option(longName: "date-of-birth", shortName: 'b', Required = true)]
	public DateTime? DateOfBirth { get; set; }

	[Option(longName: "password", shortName: 'p', Required = true)]
	public string? Password { get; set; } = string.Empty;
}
