namespace TodoList.Interfaces;

public interface IHasher
{
    string Hashed(string text);
    bool Verify(string text, string hash);
}