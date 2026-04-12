using System;
using System.Linq;
using Xunit;
using Todolist.Models;

namespace Todolist.Tests
{
    public class TodoListTests
    {
        [Fact]
        public void Constructor_CreatesEmptyList()
        {
            var list = new TodoList();
            Assert.Equal(0, list.Count);
        }

        [Fact]
        public void Add_WithValidItem_IncreasesCount()
        {
            var list = new TodoList();
            var item = new TodoItem("Test task");
            list.Add(item);
            Assert.Equal(1, list.Count);
        }

        [Fact]
        public void Remove_AtValidIndex_RemovesItem()
        {
            var list = new TodoList();
            var item = new TodoItem("Task to delete");
            list.Add(item);
            list.Add(new TodoItem("Another task"));

            bool removed = list.Remove(0);

            Assert.True(removed);
            Assert.Equal(1, list.Count);
            Assert.Equal("Another task", list.Get(0).Text);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(2)]
        public void Remove_AtInvalidIndex_ReturnsFalse(int invalidIndex)
        {
            var list = new TodoList();
            list.Add(new TodoItem("Task 1"));
            list.Add(new TodoItem("Task 2"));

            bool removed = list.Remove(invalidIndex);
            Assert.False(removed);
            Assert.Equal(2, list.Count);
        }

        [Fact]
        public void Get_WithValidIndex_ReturnsItem()
        {
            var list = new TodoList();
            var item = new TodoItem("Test task");
            list.Add(item);

            var result = list.Get(0);
            Assert.Same(item, result);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(1)]
        public void Get_WithInvalidIndex_ThrowsIndexOutOfRangeException(int invalidIndex)
        {
            var list = new TodoList();
            list.Add(new TodoItem("Task"));

            Assert.Throws<IndexOutOfRangeException>(() => list.Get(invalidIndex));
        }

        [Fact]
        public void Update_WithValidIndex_UpdatesItem()
        {
            var list = new TodoList();
            list.Add(new TodoItem("Old"));
            var newItem = new TodoItem("New");

            list.Update(0, newItem);

            Assert.Equal("New", list.Get(0).Text);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(1)]
        public void Update_WithInvalidIndex_ThrowsIndexOutOfRangeException(int invalidIndex)
        {
            var list = new TodoList();
            list.Add(new TodoItem("Task"));

            Assert.Throws<IndexOutOfRangeException>(() => list.Update(invalidIndex, new TodoItem("New")));
        }

        [Fact]
        public void Clear_RemovesAllItems()
        {
            var list = new TodoList();
            list.Add(new TodoItem("Task 1"));
            list.Add(new TodoItem("Task 2"));

            list.Clear();

            Assert.Equal(0, list.Count);
        }

        [Fact]
        public void GetEnumerator_ReturnsAllItems()
        {
            var list = new TodoList();
            var item1 = new TodoItem("Task 1");
            var item2 = new TodoItem("Task 2");
            list.Add(item1);
            list.Add(item2);

            var items = list.ToList();

            Assert.Equal(2, items.Count);
            Assert.Contains(item1, items);
            Assert.Contains(item2, items);
        }

        [Fact]
        public void Capacity_IncreasesWhenAddingBeyondCapacity()
        {
            var list = new TodoList(2);
            list.Add(new TodoItem("1"));
            list.Add(new TodoItem("2"));
            Assert.Equal(2, list.Capacity);
            list.Add(new TodoItem("3"));
            Assert.True(list.Capacity > 2);
        }
    }
}