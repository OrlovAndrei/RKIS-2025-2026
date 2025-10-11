//This file contains every command and option for program and their logic - PoneMaurice 
using System.Runtime;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Nodes;

namespace Task
{
    public class Survey
    {
        public void GlobalCommand(string Text)
        {
            string ask = Commands.InputString(Text);
            SearchCommandOnJson commandLine = new(ask.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
            switch (commandLine.commandOut)
            {
                case "add":
                    if (commandLine.optionsOut.Length > 0)
                    {
                        if (commandLine.SearchOption(["help"]))
                        {
                            Commands.AddHelp();
                        }
                        else if (commandLine.SearchOption(["task"]))
                        {
                            Commands.AddTask();
                        }
                        else if (commandLine.SearchOption(["config"]))
                        {
                            Commands.AddConfUserData(commandLine.nextTextOut);
                        }
                        else if (commandLine.SearchOption(["profile"]))
                        {
                            Commands.AddProfile();
                        }
                        else {Commands.AddUserData(commandLine.nextTextOut);}
                    }
                    else {Commands.AddUserData(commandLine.nextTextOut);}
                    break;

                case "profile":
                    if (commandLine.optionsOut.Length > 0)
                    {
                        if (commandLine.SearchOption(["help"]))
                        {
                            Commands.ProfileHelp();
                        }
                        else if (commandLine.SearchOption(["add"]))
                        {
                            Commands.AddProfile();
                        }
                    }
                    else {} ///////////////////////////////////////////////////////////
                    break;

                case "print":
                    if (commandLine.optionsOut.Length > 0)
                    {
                        if (commandLine.SearchOption(["help"]))
                        {
                            Commands.PrintHelp();
                        }
                        else if (commandLine.SearchOption(["task"]))
                        {
                            Commands.PrintData(ConstProgram.TaskName);
                        }
                        else if (commandLine.SearchOption(["config"]))
                        {
                            Commands.PrintData(commandLine.nextTextOut + ConstProgram.PrefConfigFile);
                        }
                        else if (commandLine.SearchOption(["profile"]))
                        {
                            Commands.PrintData(ConstProgram.ProfileName);
                        }
                        else if (commandLine.SearchOption(["captions"])){
                            Commands.WriteCaption();
                        } 
                        else { Commands.PrintData(commandLine.nextTextOut); }
                    }
                    else { Commands.PrintData(commandLine.nextTextOut); }
                    break;

                case "search":
                    if (commandLine.optionsOut.Length > 0)
                    {
                        if (commandLine.SearchOption(["help"]))
                        {
                            Commands.SearchHelp();
                        }
                        else if (commandLine.SearchOption(["task"]))
                        {

                        }
                        else if (commandLine.SearchOption(["config"]))
                        {

                        }
                        else if (commandLine.SearchOption(["profile"]))
                        {

                        }
                        else if (commandLine.SearchOption(["numbering"]))
                        {

                        }
                        else { }
                    }
                    else {Commands.SearchPartData();}
                    break;

                case "clear":
                    if (commandLine.optionsOut.Length > 0)
                    {
                        if (commandLine.SearchOption(["help"]))
                        {

                        }
                        else if (commandLine.SearchOption(["task"]))
                        {

                        }
                        else if (commandLine.SearchOption(["config"]))
                        {

                        }
                        else if (commandLine.SearchOption(["profile"]))
                        {

                        }
                        else if (commandLine.SearchOption(["console"]))
                        {
                            System.Console.WriteLine("CCCCCCClear");
                            Console.Clear();
                        }
                        else { }
                    }
                    else { }
                    break;

                case "help":
                    Commands.Help();
                    break;

                case "exit":
                    Environment.Exit(0);
                    break;
            }
            
        }
    }
}