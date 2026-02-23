namespace Application.Interfaces;

public interface IUndoRedo : IUseCaseExecute
{
	Task<int> Undo();
}