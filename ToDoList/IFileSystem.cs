using System.Collections.Generic;

namespace TodoList
{
	public interface IFileSystem
	{
		bool Exists(string path);
		void WriteAllText(string path, string contents);
		string ReadAllText(string path);
		void WriteAllLines(string path, string[] contents);
		string[] ReadAllLines(string path);
		void CreateDirectory(string path);
		bool DirectoryExists(string path);
	}
}