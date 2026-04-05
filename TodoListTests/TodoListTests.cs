using System;
using System.Linq;
using Xunit;

namespace Todolist.Tests
{
    public class TodoListTests
    {
        [Fact]
        public void Constructor_CreatesEmptyList()
        {
            var list = new Todolist();

            Assert.Equal(0, list.GetCount());
        }

        [Fact]
        public void Add_WithValidItem_IncreasesCount()
        {
            var list = new Todolist();
            var item = new TodoItem("Test task");

            list.Add(item);

            Assert.Equal(1, list.GetCount());
        }

        [Fact]
        public void Insert_AtValidIndex_InsertsItem()
        {
            var list = new Todolist();
            list.Add(new TodoItem("Task 1"));
            list.Add(new TodoItem("Task 3"));
            var itemToInsert = new TodoItem("Task 2");

            list.Insert(itemToInsert, 1);

            Assert.Equal(3, list.GetCount());
            Assert.Equal("Task 2", list.GetItem(1).Text);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(5)]
        public void Insert_AtInvalidIndex_ThrowsArgumentOutOfRangeException(int invalidIndex)
        {
            var list = new Todolist();
            list.Add(new TodoItem("Task 1"));

            Assert.Throws<ArgumentOutOfRangeException>(() => list.Insert(new TodoItem("Task"), invalidIndex));
        }

        [Fact]
        public void Delete_AtValidIndex_RemovesItem()
        {
            var list = new Todolist();
            var item = new TodoItem("Task to delete");
            list.Add(item);
            list.Add(new TodoItem("Another task"));

            list.Delete(0);

            Assert.Equal(1, list.GetCount());
            Assert.Equal("Another task", list.GetItem(0).Text);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(2)]
        public void Delete_AtInvalidIndex_ThrowsArgumentOutOfRangeException(int invalidIndex)
        {
            var list = new Todolist();
            list.Add(new TodoItem("Task 1"));
            list.Add(new TodoItem("Task 2"));

            Assert.Throws<ArgumentOutOfRangeException>(() => list.Delete(invalidIndex));
        }

        [Fact]
        public void SetStatus_WithValidIndex_UpdatesStatus()
        {
            var list = new Todolist();
            var item = new TodoItem("Task");
            list.Add(item);

            list.SetStatus(0, TodoStatus.InProgress);

            Assert.Equal(TodoStatus.InProgress, item.Status);
        }

        [Fact]
        public void SetStatus_WithValidIndexAndUpdateTimeFalse_DoesNotUpdateTime()
        {
            var list = new Todolist();
            var item = new TodoItem("Task");
            var oldLastUpdate = item.LastUpdate;
            list.Add(item);

            list.SetStatus(0, TodoStatus.InProgress, false);

            Assert.Equal(TodoStatus.InProgress, item.Status);
            Assert.Equal(oldLastUpdate, item.LastUpdate);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(5)]
        public void SetStatus_WithInvalidIndex_ThrowsArgumentOutOfRangeException(int invalidIndex)
        {
            var list = new Todolist();
            list.Add(new TodoItem("Task"));

            Assert.Throws<ArgumentOutOfRangeException>(() => list.SetStatus(invalidIndex, TodoStatus.InProgress));
        }

        [Fact]
        public void GetItem_WithValidIndex_ReturnsItem()
        {

            var list = new Todolist();
            var item = new TodoItem("Test task");
            list.Add(item);

            var result = list.GetItem(0);

            Assert.Same(item, result);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(1)]
        public void GetItem_WithInvalidIndex_ThrowsArgumentOutOfRangeException(int invalidIndex)
        {
            var list = new Todolist();
            list.Add(new TodoItem("Task"));

            Assert.Throws<ArgumentOutOfRangeException>(() => list.GetItem(invalidIndex));
        }

        [Fact]
        public void Indexer_WithValidIndex_ReturnsItem()
        {
            var list = new Todolist();
            var item = new TodoItem("Test task");
            list.Add(item);

            var result = list[0];

            Assert.Same(item, result);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(1)]
        public void Indexer_WithInvalidIndex_ThrowsArgumentOutOfRangeException(int invalidIndex)
        {
            var list = new Todolist();
            list.Add(new TodoItem("Task"));

            Assert.Throws<ArgumentOutOfRangeException>(() => list[invalidIndex]);
        }

        [Fact]
        public void GetEnumerator_ReturnsAllItems()
        {
            var list = new Todolist();
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
        public void View_WithEmptyList_DoesNotThrowException()
        {
            var list = new Todolist();

            var exception = Record.Exception(() => list.View(false, false, false));
            Assert.Null(exception);
        }

        [Fact]
        public void MultipleOperations_WorkCorrectly()
        {
            var list = new Todolist();

            list.Add(new TodoItem("Task 1"));
            list.Add(new TodoItem("Task 2"));
            list.Insert(new TodoItem("Task 1.5"), 1);
            list.SetStatus(2, TodoStatus.Completed);
            list.Delete(0);

            Assert.Equal(2, list.GetCount());
            Assert.Equal("Task 1.5", list.GetItem(0).Text);
            Assert.Equal("Task 2", list.GetItem(1).Text);
            Assert.Equal(TodoStatus.Completed, list.GetItem(1).Status);
        }
    }
} 