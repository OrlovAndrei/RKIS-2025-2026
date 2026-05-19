using System;
using TodoList;
using ToDoList;
using Xunit;

namespace TodoList.Tests
{
	public class ProfileTests
	{
		[Fact]
		public void GetInfo_ValidData_ReturnsCorrectString()
		{
			var profile = new Profile("Илья", "Иванов", 2002);
			int age = DateTime.Now.Year - 2002;
			string expected = "Илья Иванов, возраст " + age;

			string result = profile.GetInfo();

			Assert.Equal(expected, result);
		}
	}
}