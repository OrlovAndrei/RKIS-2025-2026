namespace Application.UseCase;

public interface IUndoRedo : IUseCaseExecute
{
    Task<int> Undo();
}