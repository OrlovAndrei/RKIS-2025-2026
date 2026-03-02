using CommandLine;

namespace ConsoleApp.Parser.Verb;

[Verb(name: "profile-list", aliases: ["pl"])]
internal class ProfileList
{
	[Option(longName: "top", shortName: 't')]
	public int? Top { get; set; }
}
