namespace ShevricTodo.Formats;

public interface ISerializationFormat<T>
{
	Task SerializationAsync(T values);
	Task<T?> DeserializationAsync();
	Task<string> StringInfoAsync();
}
