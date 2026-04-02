using System;

namespace TodoList;

public class HelpCommand : ICommand
{
    public void Execute()
    {
        Console.WriteLine("""
        Доступные команды:
        help — справка
        profile [--out|-o] — информация о профиле или выход
        add "текст" [--multiline|-m] — добавить задачу
        view [--index|-i] [--status|-s] [--update-date|-d] [--all|-a] — просмотр
        status индекс статус — изменить статус
        delete индекс — удалить задачу
        update индекс "новый текст" — обновить текст
        read индекс — полная информация
        undo — отменить
        redo — повторить
        exit — выход из программы
        """);
    }

    public void Unexecute() { }
}