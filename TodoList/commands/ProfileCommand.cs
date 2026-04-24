using System;
using System.IO;
using System.Linq;

namespace TodoList
{
    /// <summary>
    /// Команда вывода информации о профиле пользователя.
    /// </summary>
    internal class ProfileCommand : ICommand
    {
        public bool Logout { get; set; }

        public void Execute()
        {
            if (Logout)
            {
                if (AppInfo.CurrentProfileId == null)
                {
                    Console.WriteLine("Вы не вошли в профиль.");
                    return;
                }

                // Сохраняем заметки перед выходом
                if (AppInfo.CurrentProfileId != null && AppInfo.TodosByProfile.ContainsKey(AppInfo.CurrentProfileId.Value))
                {
                    string todoPath = Path.Combine(AppInfo.DataDirectory, $"todos_{AppInfo.CurrentProfileId}.csv");
                    FileManager.SaveTodos(AppInfo.Todos, todoPath);
                }

                AppInfo.CurrentProfileId = null;
                AppInfo.UndoStack.Clear();
                AppInfo.RedoStack.Clear();
                Console.WriteLine("Выход из профиля выполнен.");
            }
            else
            {
                if (AppInfo.CurrentProfileId == null)
                {
                    Console.WriteLine("Ошибка: профиль не установлен.");
                    return;
                }

                var profile = AppInfo.Profiles.FirstOrDefault(p => p.Id == AppInfo.CurrentProfileId);
                if (profile == null)
                {
                    Console.WriteLine("Ошибка: профиль не найден.");
                    return;
                }

                Console.WriteLine(profile.GetInfo());
            }
        }

        public void Unexecute()
        {
            // Команда profile ничего не изменяет (кроме выхода, но отмена выхода не требуется)
        }
    }
}

