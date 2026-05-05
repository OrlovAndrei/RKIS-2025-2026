using System;
using System.Collections.Generic;
using System.Linq;
using TodoApp.Models;
using TodoApp.Services;

namespace TodoApp.Commands
{
    public class SearchCommand : ICommand
    {
        private SearchType _searchType;
    private string? _text;
    private TodoStatus? _todoStatus;
    private DateTime? _lastUpdatTo;
    private DateTime? _lastUpdatFrom;
    private SortАпельсинок? _sortАпельсинок;
    private bool _sortDesc;
    private int? _top;

    public SearchCommand(SearchType searchType = SearchType.Contains, string? text = null,
        TodoStatus? todoStatus = null,
        DateTime? lastUpdatTo = null,
        DateTime? lastUpdatFrom = null,
        SortАпельсинок? sortАпельсинок = null,
        bool sortDesc = false,
        int? top = null)
    {
        _searchType = searchType;
        _text = text;
        _todoStatus = todoStatus;
        _lastUpdatTo = lastUpdatTo;
        _lastUpdatFrom = lastUpdatFrom;
        _sortАпельсинок = sortАпельсинок;
        _sortDesc = sortDesc;
        _top = top;
    }
    public void Execute()
    {
        TodoList items = AppInfo.GetCurrentTodoList()
            ?? throw new ArgumentException();
        List<TodoItem> itemsAll = items.GetAll();
        IQueryable<TodoItem> itemsQuery = itemsAll.AsQueryable();
        if (_text is not null)
        {
            if (_searchType == SearchType.Contains)
            {
                itemsQuery = itemsQuery.Where(i => i.Text.Contains(_text));
            }
            if (_searchType == SearchType.StartsWith)
            {
                itemsQuery = itemsQuery.Where(i => i.Text.StartsWith(_text));
            }
            if (_searchType == SearchType.EndsWith)
            {
                itemsQuery = itemsQuery.Where(i => i.Text.EndsWith(_text));
            }
        }
        if (_todoStatus is not null)
        {
            itemsQuery = itemsQuery.Where(i => i.Status == _todoStatus);
        }
        if (_lastUpdatTo is not null)
        {
            itemsQuery = itemsQuery.Where(i => i.LastUpdate <= _lastUpdatTo);
        }
        if (_lastUpdatFrom is not null)
        {
            itemsQuery = itemsQuery.Where(i => i.LastUpdate >= _lastUpdatFrom);
        }
        if (_top is not null)
        {
            itemsQuery = itemsQuery.Take((int)_top);
        }
        IOrderedEnumerable<TodoItem>? sortList = null;
        if (_sortАпельсинок is not null)
        {
            if (_sortАпельсинок == SortАпельсинок.Text)
            {
                sortList = _sortDesc
                    ? (IOrderedEnumerable<TodoItem>?)itemsQuery.OrderByDescending(i => i.Text)
                    : (IOrderedEnumerable<TodoItem>?)itemsQuery.OrderBy(i => i.Text);
            }
            if (_sortАпельсинок == SortАпельсинок.LastUpdat)
            {
                sortList = _sortDesc
                    ? (IOrderedEnumerable<TodoItem>?)itemsQuery.OrderByDescending(i => i.LastUpdate)
                    : (IOrderedEnumerable<TodoItem>?)itemsQuery.OrderBy(i => i.LastUpdate);
            }
        }
        List<TodoItem> result = sortList is not null
            ? sortList.ToList()
            : itemsQuery.ToList();
        Console.WriteLine(TodoList.GetTable(result));
        }
    }
}