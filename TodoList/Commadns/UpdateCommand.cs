using System;
using System.Collections.Generic;

namespace TodoList
{
    public class UpdateCommand : ICommand
    {
        public int TaskNumber { get; set; }
        public string NewText { get; set; }
        public TodoList TodoList { get; set; }
        public bool IsMultiline { get; set; }

        public void Execute()
        {
            try
            {
                var item = TodoList.GetItem(TaskNumber);
                string newText = NewText;

                if (IsMultiline)
                {
                    newText = ReadMultilineText();
                    if (string.IsNullOrWhiteSpace(newText))
                    {
                        Console.WriteLine("Обновление задачи отменено.");
                        return;
                    }
                }
                else
                {
                    if (newText.StartsWith("\"") && newText.EndsWith("\""))
                    {
                        newText = newText.Substring(1, newText.Length - 2);
                    }
                }

                string oldText = item.Text;
                item.UpdateText(newText, IsMultiline);
                
                if (IsMultiline)
                {
                    Console.WriteLine($"Задача {TaskNumber} обновлена (многострочная):");
                    Console.WriteLine("Новый текст:");
                    Console.WriteLine(new string('-', 40));
                    Console.WriteLine(newText);
                    Console.WriteLine(new string('-', 40));
                }
                else
                {
                    Console.WriteLine($"Задача обновлена: \nБыло: №{TaskNumber} \"{oldText}\" \nСтало: №{TaskNumber} \"{newText}\"");
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private string ReadMultilineText()
        {
            Console.WriteLine("Введите новый текст задачи (для завершения введите пустую строку):");
            Console.WriteLine("(Подсказка: нажмите Enter дважды для завершения ввода)");
            
            List<string> lines = new List<string>();
            string line;
            int emptyLineCount = 0;

            while (true)
            {
                Console.Write("> ");
                line = Console.ReadLine();

                if (string.IsNullOrEmpty(line))
                {
                    emptyLineCount++;
                    if (emptyLineCount >= 2 || lines.Count == 0)
                    {
                        break;
                    }
                    lines.Add("");
                }
                else
                {
                    emptyLineCount = 0;
                    lines.Add(line);
                }
            }

            string result = string.Join("\n", lines);
            return result.Trim();
        }
    }
}