using System;
using System.Windows;
using TodoApp.Desktop.ViewModels;
using TodoApp.Models;

namespace TodoApp.Desktop.Views;

public partial class AddTaskWindow : Window
{
    public AddTaskWindow()
    {
        InitializeComponent();
        var vm = new AddTaskViewModel(Guid.Empty);
        DataContext = vm;
        vm.CloseRequest += (saved) => { DialogResult = saved; Close(); };
        StatusComboBox.ItemsSource = Enum.GetValues(typeof(TodoStatus));
    }
}