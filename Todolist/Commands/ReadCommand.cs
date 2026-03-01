using System;
using Todolist.Exceptions;

namespace Todolist.Commands
{
    internal class ReadCommand : ICommand
    {
        public int Index { get; set; }

        public ReadCommand(int index)
        {
            Index = index;
        }

        public void Execute()
        {
            if (AppInfo.CurrentProfileId == Guid.Empty)
                throw new AuthenticationException("Необходимо войти в профиль для работы с задачами.");
            AppInfo.Todos.Read(Index);
        }

        public void Unexecute()
        {
            // команда только читает данные, откат не нужен
        }
    }
}

