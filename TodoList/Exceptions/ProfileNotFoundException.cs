using System;
namespace TodoApp.Exceptions
{
    public class ProfileNotFoundException : System.Exception
    {
        public ProfileNotFoundException(string message) : base(message) { }
    }
}