using System;
public class Profile
{
    public string FirstName;
    public string LastName;
    public int BirthYear;
    public Profile(string firstName, string lastName, int birthYear)
    {
        FirstName = firstName;
        LastName = lastName;
        BirthYear = birthYear;
    }
    public string GetInfo() => $"{FirstName}, {LastName}, {BirthYear}";
}