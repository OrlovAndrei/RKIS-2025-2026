namespace TodoList
{
    public static class CommandParser
    {
        public static ICommand Parse(string inputString, TodoList todoList, Profile profile)
        {
            if (string.IsNullOrWhiteSpace(inputString))
                return new UnknownCommand();

            string[] inputParts = inputString.Split(' ');
            if (inputParts == null || inputParts.Length == 0)
                return new UnknownCommand();

            string command = inputParts[0].ToLower();

            if (command == "help")
            {
                return new HelpCommand();
            }
            else if (command == "profile")
            {
                bool skipProfile = false;
                for (int i = 1; i < inputParts.Length; i++)
                {
                    if (inputParts[i] == "--skip-profile" || inputParts[i] == "-s")
                    {
                        skipProfile = true;
                    }
                }
                return new ProfileCommand { Profile = profile, SkipProfile = skipProfile };
            }
            else if (command == "view")
            {
                bool showIndex = false;
                bool showStatus = false;
                bool showDate = false;

                for (int i = 1; i < inputParts.Length; i++)
                {
                    if (inputParts[i] == "--index" || inputParts[i] == "-i" || inputParts[i] == "-is" || inputParts[i] == "-dis")
                    {
                        showIndex = true;
                    }
                    else if (inputParts[i] == "--status" || inputParts[i] == "-s" || inputParts[i] == "-is" || inputParts[i] == "-ds" || inputParts[i] == "-dis")
                    {
                        showStatus = true;
                    }
                    else if (inputParts[i] == "--update-date" || inputParts[i] == "-d" || inputParts[i] == "-ds" || inputParts[i] == "-dis")
                    {
                        showDate = true;
                    }
                    else if (inputParts[i] == "--all" || inputParts[i] == "-a")
                    {
                        showIndex = true;
                        showStatus = true;
                        showDate = true;
                    }
                }

                return new ViewCommand { TodoList = todoList, ShowIndex = showIndex, ShowStatus = showStatus, ShowDate = showDate };
            }
            else if (command == "link")
            {
                return new LinkCommand();
            }
            else if (command == "exit")
            {
                return new ExitCommand();
            }
            else if (command.StartsWith("add"))
            {
                bool multiline = false;
                for (int i = 1; i < inputParts.Length; i++)
                {
                    if (inputParts[i] == "--multiline" || inputParts[i] == "-m")
                    {
                        multiline = true;
                    }
                }

                string taskText = ExtractTaskText(inputString);
                return new AddCommand { TodoList = todoList, TaskText = taskText, Multiline = multiline };
            }
            else if (command.StartsWith("done"))
            {
                int taskIndex = ExtractTaskIndex(inputString, "done");
                return new DoneCommand { TodoList = todoList, TaskIndex = taskIndex };
            }
            else if (command.StartsWith("delete"))
            {
                int taskIndex = ExtractTaskIndex(inputString, "delete");
                bool force = false;
                for (int i = 1; i < inputParts.Length; i++)
                {
                    if (inputParts[i] == "--force" || inputParts[i] == "-f")
                    {
                        force = true;
                    }
                }
                return new DeleteCommand { TodoList = todoList, TaskIndex = taskIndex, Force = force };
            }
            else if (command.StartsWith("update"))
            {
                int taskIndex = ExtractTaskIndex(inputString, "update");
                bool force = false;
                for (int i = 1; i < inputParts.Length; i++)
                {
                    if (inputParts[i] == "--force" || inputParts[i] == "-f")
                    {
                        force = true;
                    }
                }
                string newText = ExtractUpdateText(inputString);
                return new UpdateCommand { TodoList = todoList, TaskIndex = taskIndex, NewText = newText, Force = force };
            }
            else if (command.StartsWith("read"))
            {
                int taskIndex = ExtractTaskIndex(inputString, "read");
                return new ReadCommand { TodoList = todoList, TaskIndex = taskIndex };
            }
            else
            {
                return new UnknownCommand();
            }
        }

        private static string ExtractTaskText(string input)
        {
            if (input == null)
                return null;

            string[] parts = input.Split('"');
            if (parts == null || parts.Length < 2)
            {
                return null;
            }

            if (parts.Length < 2 || string.IsNullOrWhiteSpace(parts[1]))
            {
                return null;
            }

            return parts[1].Trim();
        }

        private static int ExtractTaskIndex(string input, string commandName)
        {
            if (input == null)
                return -1;

            string[] parts = input.Split(' ');
            if (parts == null || parts.Length < 2)
            {
                return -1;
            }

            int taskIndex = -1;
            for (int i = 1; i < parts.Length; i++)
            {
                if (int.TryParse(parts[i], out taskIndex))
                {
                    break;
                }
            }

            if (taskIndex < 0)
            {
                return -1;
            }

            return taskIndex;
        }

        private static string ExtractUpdateText(string input)
        {
            string[] parts = input.Split(new char[] { ' ' }, 3, StringSplitOptions.RemoveEmptyEntries);
            if (parts == null || parts.Length < 3)
            {
                return null;
            }

            if (parts.Length < 3 || !parts[2].StartsWith("\"") || !parts[2].EndsWith("\""))
            {
                return null;
            }

            return parts[2].Substring(1, parts[2].Length - 2).Trim();
        }
    }
}