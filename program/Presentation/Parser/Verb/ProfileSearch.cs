using CommandLine;

namespace Presentation.Parser.Verb;

[Verb(name: "profile-search", aliases: ["ps"])]
internal class ProfileSearch
{
	[Option(longName: "first-name", shortName: 'f')]
	public string? FirstName { get; set; }

	[Option(longName: "last-name", shortName: 'l')]
	public string? LastName { get; set; }

	[Option(longName: "date-of-birth-from", shortName: 'b')]
	public DateTime? DateOfBirthFrom { get; set; }

	[Option(longName: "date-of-birth-to", shortName: 'B')]
	public DateTime? DateOfBirthTo { get; set; }

	[Option(longName: "create-at-from", shortName: 'c')]
	public DateTime? CreateAtFrom { get; set; }

	[Option(longName: "create-at-to", shortName: 'C')]
	public DateTime? CreateAtTo { get; set; }

	[Option(longName: "top", shortName: 't')]
	public int? Top { get; set; }

	[Option(longName: "search-type")]
	public string? SearchType { get; set; }
}
