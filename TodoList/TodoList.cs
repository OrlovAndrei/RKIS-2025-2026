using System;

namespace TodoApp
{
    public class TodoList
    {
        // Приватное поле: массив задач
        private TodoItem[] items;
        private int count;

        // Конструктор
        public TodoList(int initialCapacity = 10)
        {
            items = new TodoItem[initialCapacity];
            count = 0;
        }

        // Метод для добавления задачи
        public void Add(TodoItem item)
        {
            if (count >= items.Length)
            {
                IncreaseArray(items, item);
            }
            else
            {
                items[count] = item;
                count++;
            }
        }

        // Метод для удаления задачи по индексу
        public void Delete(int index)
        {
            if (index < 0 || index >= count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Индекс находится вне диапазона");
            }

            // Сдвигаем элементы массива
            for (int i = index; i < count - 1; i++)
            {
                items[i] = items[i + 1];
            }

            items[count - 1] = null;
            count--;
        }

        // Метод для вывода задач в виде таблицы
        public void View(bool showIndex = true, bool showDone = true, bool showDate = true)
        {
            if (count == 0)
            {
                Console.WriteLine("Список задач пуст");
                return;
            }

            // Заголовок таблицы
            Console.WriteLine(new string('-', 80));
            string header = "";
            if (showIndex) header += "№".PadRight(5);
            header += "Задача".PadRight(35);
            if (showDone) header += "Статус".PadRight(15);
            if (showDate) header += "Дата изменения";
            Console.WriteLine(header);
            Console.WriteLine(new string('-', 80));

            // Вывод задач
            for (int i = 0; i < count; i++)
            {
                string row = "";
                
                if (showIndex)
                {
                    row += $"{i + 1}".PadRight(5);
                }

                // Обрезаем текст задачи до 30 символов
                string shortText = items[i].Text.Length > 30 ? 
                    items[i].Text.Substring(0, 30) + "..." : 
                    items[i].Text.PadRight(33);
                row += shortText.PadRight(35);

                if (showDone)
                {
                    string status = items[i].IsDone ? " Выполнена" : " Не выполнена";
                    row += status.PadRight(15);
                }

                if (showDate)
                {
                    row += items[i].LastUpdate.ToString("dd.MM.yyyy HH:mm");
                }

                Console.WriteLine(row);
            }
            Console.WriteLine(new string('-', 80));
        }

        // Метод для получения задачи по индексу
        public TodoItem GetItem(int index)
        {
            if (index < 0 || index >= count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Индекс находится вне диапазона");
            }
            return items[index];
        }

        // Приватный метод для увеличения размера массива
        private void IncreaseArray(TodoItem[] oldArray, TodoItem newItem)
        {
            // Увеличиваем размер массива в 2 раза
            TodoItem[] newArray = new TodoItem[oldArray.Length * 2];
            
            // Копируем старые элементы
            for (int i = 0; i < oldArray.Length; i++)
            {
                newArray[i] = oldArray[i];
            }
            
            // Добавляем новый элемент
            newArray[count] = newItem;
            count++;
            
            // Заменяем старый массив новым
            items = newArray;
        }

        // Свойство для получения количества задач
        public int Count => count;

        // Свойство для проверки, пуст ли список
        public bool IsEmpty => count == 0;
    }
}