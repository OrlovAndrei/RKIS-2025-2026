namespace TodoList
{
    public abstract class BaseCommand : ICommand
    {
        protected TodoList TodoList { get; set; }
        protected Profile Profile { get; set; }

        public BaseCommand(TodoList todoList = null, Profile profile = null)
        {
            TodoList = todoList;
            Profile = profile;
        }

        public abstract void Execute();
    }
}