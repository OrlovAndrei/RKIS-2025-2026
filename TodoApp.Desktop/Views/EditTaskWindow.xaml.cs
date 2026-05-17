using System;
using System.Windows;
using TodoApp.Desktop.ViewModels;
using TodoApp.Models;

namespace TodoApp.Desktop.Views;

public partial class EditTaskWindow : Window
{
    public EditTaskWindow(TodoItem task)
    {
        InitializeComponent();
        var vm = new EditTaskViewModel(task);
        DataContext = vm;
        vm.CloseRequest += (saved) =>
        {
            DialogResult = saved;
            Close();
        };
        StatusComboBox.ItemsSource = Enum.GetValues(typeof(TodoStatus));
        StatusComboBox.SelectedItem = vm.SelectedStatus;
    }
}