namespace TodoList
{
    public class Profile
    {
        private string _firstName;
        private string _lastName;
        private int _birthYear;

        public string FirstName => _firstName;
        public string LastName => _lastName;
        public int BirthYear => _birthYear;

        public Profile(string firstName, string lastName, int birthYear)
        {
            _firstName = firstName;
            _lastName = lastName;
            _birthYear = birthYear;
        }

        public string GetInfo()
        {
            int age = DateTime.Now.Year - _birthYear;
            return $"{_firstName} {_lastName}, возраст {age}";
        }
    }
}