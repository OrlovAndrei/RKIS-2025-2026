using System;
using TodoList;
using TodoList.Commands;
using TodoList.Exceptions;
using ToDoList;
using Xunit;

namespace TodoList.Tests
{
	public class CommandParserTests
	{
		private void PrepareApp()
		{
			AppInfo.CurrentProfile = new Profile("Илья", "Иванов", 2002);
			AppInfo.Todos = new global::TodoList.TodoList();
		}

		[Theory]
		[InlineData("help", typeof(HelpCommand))]
		[InlineData("view", typeof(ViewCommand))]
		public void Parse_ValidCommands_ReturnsCorrectType(string input, Type expectedType)
		{
			PrepareApp();

			var result = CommandParser.Parse(input);

			Assert.IsType(expectedType, result);
		}

		[Fact]
		public void Parse_NoProfile_ThrowsAuthenticationException()
		{
			AppInfo.CurrentProfile = null;

			Assert.Throws<AuthenticationException>(() => CommandParser.Parse("view"));
		}
	}
}