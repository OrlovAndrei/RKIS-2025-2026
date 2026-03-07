using System;
using System.Linq;
using Xunit;
using TodoList;

namespace TodoList.Tests
{
    public class TodoListTests
    {
        [Fact]
        public void Constructor_WithNullTasks_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new TodoList(null));
        }

        [Fact]
        public void Constructor_WithTasks_CreatesCopy()
        {
            var tasks = new System.Collections.Generic.List<TodoItem>
            {
                new TodoItem("Task1"),
                new TodoItem("Task2")
            };

            var list = new TodoList(tasks);
            tasks.Add(new TodoItem("Task3")); 

            Assert.Equal(2, list.Count);
        }

        [Fact]
        public void DefaultConstructor_InitializesEmptyList()
        {
            var list = new TodoList();

            Assert.Empty(list);
            Assert.Equal(0, list.Count);
        }

        [Fact]
        public void Add_AddsItemAndRaisesEvent()
        {
            var list = new TodoList();
            var item = new TodoItem("Test");
            TodoItem eventItem = null;
            list.OnTodoAdded += (i) => eventItem = i;

            list.Add(item);

            Assert.Single(list);
            Assert.Equal(item, list[0]);
            Assert.Equal(item, eventItem);
        }

        [Fact]
        public void Insert_ValidIndex_InsertsItemAndRaisesEvent()
        {
            var list = new TodoList();
            list.Add(new TodoItem("A"));
            var item = new TodoItem("B");
            TodoItem eventItem = null;
            list.OnTodoAdded += (i) => eventItem = i;

            list.Insert(1, item);

            Assert.Equal(2, list.Count);
            Assert.Equal(item, list[1]);
            Assert.Equal(item, eventItem);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(2)]
        public void Insert_InvalidIndex_DoesNotInsert(int index)
        {
            var list = new TodoList();
            list.Add(new TodoItem("A"));
            var item = new TodoItem("B");
            var eventRaised = false;
            list.OnTodoAdded += (i) => eventRaised = true;

            list.Insert(index, item);

            Assert.Single(list);
            Assert.False(eventRaised);
        }

        [Fact]
        public void Delete_ValidIndex_RemovesItemAndReturnsTrueAndRaisesEvent()
        {
            var list = new TodoList();
            var item = new TodoItem("Test");
            list.Add(item);
            TodoItem deletedItem = null;
            list.OnTodoDeleted += (i) => deletedItem = i;

            var result = list.Delete(1);

            Assert.True(result);
            Assert.Empty(list);
            Assert.Equal(item, deletedItem);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(2)]
        public void Delete_InvalidIndex_ReturnsFalse(int index)
        {
            var list = new TodoList();
            list.Add(new TodoItem("Test"));
            var eventRaised = false;
            list.OnTodoDeleted += (i) => eventRaised = true;

            var result = list.Delete(index);

            Assert.False(result);
            Assert.Single(list);
            Assert.False(eventRaised);
        }

        [Fact]
        public void Indexer_ValidIndex_ReturnsItem()
        {
            var list = new TodoList();
            var item = new TodoItem("Test");
            list.Add(item);

            var result = list[0];

            Assert.Equal(item, result);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(1)]
        public void Indexer_InvalidIndex_ThrowsArgumentOutOfRangeException(int index)
        {
            var list = new TodoList();
            list.Add(new TodoItem("Test"));

            Assert.Throws<ArgumentOutOfRangeException>(() => list[index]);
        }

        [Fact]
        public void SetStatus_ValidIndex_ChangesStatusAndRaisesEvent()
        {
            var list = new TodoList();
            var item = new TodoItem("Test");
            list.Add(item);
            var newStatus = TodoStatus.Completed;
            TodoItem changedItem = null;
            list.OnStatusChanged += (i) => changedItem = i;

            list.SetStatus(1, newStatus);

            Assert.Equal(newStatus, item.Status);
            Assert.Equal(item, changedItem);
        }

        [Fact]
        public void SetStatus_InvalidIndex_DoesNothing()
        {
            var list = new TodoList();
            var item = new TodoItem("Test");
            list.Add(item);
            var oldStatus = item.Status;
            var eventRaised = false;
            list.OnStatusChanged += (i) => eventRaised = true;

            list.SetStatus(2, TodoStatus.Completed);

            Assert.Equal(oldStatus, item.Status);
            Assert.False(eventRaised);
        }

        [Fact]
        public void UpdateText_ValidIndex_ChangesTextAndRaisesEvent()
        {
            var list = new TodoList();
            var item = new TodoItem("Old");
            list.Add(item);
            var newText = "New";
            TodoItem updatedItem = null;
            list.OnTodoUpdated += (i) => updatedItem = i;

            list.UpdateText(1, newText);

            Assert.Equal(newText, item.Text);
            Assert.Equal(item, updatedItem);
        }

        [Fact]
        public void UpdateText_InvalidIndex_DoesNothing()
        {
            var list = new TodoList();
            var item = new TodoItem("Old");
            list.Add(item);
            var oldText = item.Text;
            var eventRaised = false;
            list.OnTodoUpdated += (i) => eventRaised = true;

            list.UpdateText(2, "New");

            Assert.Equal(oldText, item.Text);
            Assert.False(eventRaised);
        }

        [Fact]
        public void Count_ReturnsCorrectNumber()
        {
            var list = new TodoList();
            list.Add(new TodoItem("1"));
            list.Add(new TodoItem("2"));

            Assert.Equal(2, list.Count);
        }

        [Fact]
        public void GetEnumerator_EnumeratesAllItems()
        {
            var list = new TodoList();
            var item1 = new TodoItem("1");
            var item2 = new TodoItem("2");
            list.Add(item1);
            list.Add(item2);

            var items = list.ToList();

            Assert.Equal(2, items.Count);
            Assert.Contains(item1, items);
            Assert.Contains(item2, items);
        }
    }
}