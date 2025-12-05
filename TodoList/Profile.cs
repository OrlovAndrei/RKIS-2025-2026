namespace TodoList
{
    public class Profile
    {
        public Guid Id { get; private set; }
        public string Login { get; private set; }
        public string Password { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public int BirthYear { get; private set; }
        public int Age => DateTime.Now.Year - BirthYear;

        public Profile(Guid id, string login, string password, string firstName, string lastName, int birthYear)
        {
            Id = id;
            Login = login;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            BirthYear = birthYear;
        }

        public Profile(string firstName, string lastName, int birthYear)
        {
            Id = Guid.NewGuid();
            Login = "user"; 
            Password = "password"; 
            FirstName = firstName;
            LastName = lastName;
            BirthYear = birthYear;
        }

        public string Name => $"{FirstName} {LastName}";

        public string GetInfo()
        {
            return $"{FirstName} {LastName}, возраст {Age} (логин: {Login})";
        }

        public bool CheckPassword(string password) => Password == password;

        public void Update(string firstName, string lastName, int birthYear)
        {
            FirstName = firstName;
            LastName = lastName;
            BirthYear = birthYear;
        }
    }
}