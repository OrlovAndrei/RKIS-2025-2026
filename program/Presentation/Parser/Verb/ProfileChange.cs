using CommandLine;

namespace Presentation.Parser.Verb;

[Verb(name: "profile-change", aliases: ["pc"])]
internal class ProfileChange
{
	[Option(longName: "search-first-name", shortName: 'F')]
	public string? FirstNameSearch { get; set; }

	[Option(longName: "search-last-name", shortName: 'L')]
	public string? LastNameSearch { get; set; }

	[Option(longName: "date-of-birth-from", shortName: 'b')]
	public DateTime? DateOfBirthFromSearch { get; set; }

	[Option(longName: "date-of-birth-to", shortName: 'B')]
	public DateTime? DateOfBirthToSearch { get; set; }

	[Option(longName: "create-at-from", shortName: 'c')]
	public DateTime? CreateAtFromSearch { get; set; }

	[Option(longName: "create-at-to", shortName: 'C')]
	public DateTime? CreateAtToSearch { get; set; }

	[Option(longName: "password", shortName: 'p')]
	public string? Password { get; set; }
}
