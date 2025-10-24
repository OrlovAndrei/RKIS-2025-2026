using System;

namespace TodoList
{
    class Profile
    {
        private string firstName;
        private string lastName;
        private int birthYear;

        public string FirstName
        {
            get { return firstName; }
            private set { firstName = value; }
        }
        public string LastName
        {
            get { return lastName; }
            private set { lastName = value; }
        }
        public int BirthYear
        {
            get { return birthYear; }
            private set { birthYear = value; }
        }

        public Profile(string firstName, string lastName, int birthYear)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.birthYear = birthYear;
        }

        public string GetInfo()
        {
            int age = DateTime.Now.Year - birthYear;
            return $"{firstName} {lastName}, возраст {age}";
        }
    }
}

