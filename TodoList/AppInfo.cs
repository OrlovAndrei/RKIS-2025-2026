namespace TodoList
{
    public static class AppInfo
    {
        public static TodoList Todos { get; set; }
        public static Profile CurrentProfile { get; set; }
        public static string TodoFilePath { get; set; } = "data/tasks.csv";
        public static string ProfileFilePath { get; set; } = "data/profile.json";
    }
}