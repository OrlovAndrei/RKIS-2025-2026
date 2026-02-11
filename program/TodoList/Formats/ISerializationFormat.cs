namespace ShevricTodo.Formats;

public interface ISerializationFormat<T>
{
	abstract void Serialization(T values);
	abstract T? Deserialization();
	abstract string StringInfo();
}
