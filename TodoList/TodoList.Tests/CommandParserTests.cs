using System;
using System.Collections.Generic;
using System.Text;


using Xunit;
using TodoList; 

namespace TodoList.Tests
{
	public class CommandParserTests
	{
		// ========== ТЕСТ 1: Parse с командой "add" ==========
		[Fact]
		public void Parse_AddCommand_ReturnsADD()
		{
			// Arrange
			string input = "add задача";

			// Act
			string result = CommandParser.Parse(input);

			// Assert
			Assert.Equal("ADD", result);
		}

		// ========== ТЕСТ 2: Parse с командой "remove" ==========
		[Fact]
		public void Parse_RemoveCommand_ReturnsREMOVE()
		{
			// Arrange
			string input = "remove 123";

			// Act
			string result = CommandParser.Parse(input);

			// Assert
			Assert.Equal("REMOVE", result);
		}

		// ========== ТЕСТ 3: Parse с пробелами в начале ==========
		[Fact]
		public void Parse_CommandWithLeadingSpaces_ReturnsUppercaseCommand()
		{
			// Arrange
			string input = "   list all tasks";

			// Act
			string result = CommandParser.Parse(input);

			// Assert
			Assert.Equal("LIST", result);
		}

		// ========== ТЕСТ 4: Parse с пустой строкой ==========
		[Theory]
		[InlineData("")]
		[InlineData("   ")]
		[InlineData(null)]
		public void Parse_EmptyOrWhitespace_ReturnsEMPTY(string input)
		{
			// Act
			string result = CommandParser.Parse(input);

			// Assert
			Assert.Equal("EMPTY", result);
		}
	}
}