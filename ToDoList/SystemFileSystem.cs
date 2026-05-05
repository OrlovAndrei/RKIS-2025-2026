using System.IO;

namespace TodoList
{
	public class SystemFileSystem : IFileSystem
	{
		public bool Exists(string path) => File.Exists(path);
		public void WriteAllText(string path, string contents) => File.WriteAllText(path, contents);
		public string ReadAllText(string path) => File.ReadAllText(path);
		public void WriteAllLines(string path, string[] contents) => File.WriteAllLines(path, contents);
		public string[] ReadAllLines(string path) => File.ReadAllLines(path);
		public void CreateDirectory(string path) => Directory.CreateDirectory(path);
		public bool DirectoryExists(string path) => Directory.Exists(path);
	}
}