namespace TodoApp;

public interface ICommand
{
	string Name { get; }
	string Description { get; }
	bool Execute();
}