namespace TodoList
{
    public class Profile
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public int BirthYear { get; private set; }
        public int Age => DateTime.Now.Year - BirthYear;

        public Profile(string firstName, string lastName, int birthYear)
        {
            FirstName = firstName;
            LastName = lastName;
            BirthYear = birthYear;
        }

        public string GetInfo()
        {
            return $"{FirstName} {LastName}, возраст {Age}";
        }

        public void Update(string firstName, string lastName, int birthYear)
        {
            FirstName = firstName;
            LastName = lastName;
            BirthYear = birthYear;
        }
    }
}