using Xunit;
using System;
using System.Linq;

namespace TodoList.Tests.Models
{
    public class TodoListTests
    {
        [Fact]
        public void Add_NewItem_IncreasesCountAndRaisesEvent()
        {
            var list = new TodoList();
            var item = new TodoItem("Тестовая задача");
            bool eventRaised = false;
            list.OnTodoAdded += (i) => eventRaised = true;

            list.Add(item);

            Assert.Equal(1, list.Count);
            Assert.True(eventRaised);
        }

        [Fact]
        public void GetItem_ValidIndex_ReturnsCorrectItem()
        {
            var list = new TodoList();
            var item = new TodoItem("Задача 1");
            list.Add(item);

            var result = list.GetItem(0);

            Assert.Equal(item, result);
        }

        [Fact]
        public void GetItem_InvalidIndex_ThrowsArgumentOutOfRangeException()
        {
            var list = new TodoList();

            Assert.Throws<ArgumentOutOfRangeException>(() => list.GetItem(0));
        }

        [Fact]
        public void Delete_ValidIndex_RemovesItemAndRaisesEvent()
        {

            var list = new TodoList();
            var item = new TodoItem("Задача 1");
            list.Add(item);
            bool eventRaised = false;
            list.OnTodoDeleted += (i) => eventRaised = true;

            list.Delete(0);

            Assert.Equal(0, list.Count);
            Assert.True(eventRaised);
        }

        [Fact]
        public void SetStatus_ValidIndex_ChangesStatusAndRaisesEvent()
        {
            var list = new TodoList();
            list.Add(new TodoItem("Задача 1"));
            bool eventRaised = false;
            list.OnStatusChanged += (i) => eventRaised = true;

            list.SetStatus(0, TodoStatus.Completed);

            var item = list.GetItem(0);
            Assert.Equal(TodoStatus.Completed, item.Status);
            Assert.True(eventRaised);
        }

        [Fact]
        public void UpdateText_ValidIndex_UpdatesTextAndRaisesEvent()
        {
            var list = new TodoList();
            list.Add(new TodoItem("Старый текст"));
            bool eventRaised = false;
            list.OnTodoUpdated += (i) => eventRaised = true;

            list.UpdateText(0, "Новый текст");

            var item = list.GetItem(0);
            Assert.Equal("Новый текст", item.Text);
            Assert.True(eventRaised);
        }

        [Fact]
        public void View_EmptyList_DoesNotThrowException()
        {
            var list = new TodoList();

            var exception = Record.Exception(() => list.View(false, false, false));
            Assert.Null(exception);
        }
    }
}