using CommandLine;

namespace ConsoleApp.Parser.Verb;

[Verb(name: "profile-edit", aliases: ["pe"])]
internal class ProfileEdit
{
	#region search
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
	#endregion

	#region execute
	[Option(longName: "first-name", shortName: 'f')]
	public string? FirstName { get; set; }

	[Option(longName: "last-name", shortName: 'l')]
	public string? LastName { get; set; }

	[Option(longName: "date-of-birth", shortName: 'd')]
	public DateTime? DateOfBirth { get; set; }

	[Option(longName: "password", shortName: 'p')]
	public string? Password { get; set; }
	#endregion
}
