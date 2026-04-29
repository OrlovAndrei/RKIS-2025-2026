using System;
using Xunit;
using TodoList;

namespace TodoList.Tests
{
	public class TodoListTests
	{
		[Fact]
		public void Add_NewItem_CountIncreases()
		{
			global::TodoList.TodoList list = new global::TodoList.TodoList();
			TodoItem item = new TodoItem("Задача Ильи");

			list.Add(item);

			Assert.Equal(1, list.Count);
		}

		[Fact]
		public void GetItem_InvalidIndex_ThrowsException()
		{
			global::TodoList.TodoList list = new global::TodoList.TodoList();

			Assert.Throws<ArgumentException>(() => (object)list.GetItem(999));
		}
	}
}