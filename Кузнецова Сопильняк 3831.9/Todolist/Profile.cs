namespace TodoList
{
    class Profile
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int BirthYear { get; set; }

        public string GetInfo()
        {
            return $"{FirstName} {LastName}, возраст {BirthYear}";
        }
    }
}