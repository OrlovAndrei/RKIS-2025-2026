namespace ShevricTodo.Commands;

internal interface ICommand<TResult, TObject>
{
	System.Threading.Tasks.Task<(TResult, TObject)> Done(params object[] objects);
}
