using System;
using TodoList.Exceptions;

namespace TodoList.Commands
{
    public class ReadCommand : ICommand
    {
        public string Arg { get; set; }

        public void Execute()
        {
            var todos = AppInfo.CurrentUserTodos;
            if (todos == null)
            {
                throw new AuthenticationException("Не удалось получить список задач. Войдите в профиль.");
            }

            todos.ReadTask(Arg);
        }

        public void Unexecute() { }
    }
}