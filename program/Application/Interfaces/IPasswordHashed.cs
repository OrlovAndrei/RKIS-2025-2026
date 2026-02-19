namespace Application.Interfaces;

public interface IPasswordHashed
{
    string Hashed(string password);
    bool Verify(string password, string hashedPassword);
}