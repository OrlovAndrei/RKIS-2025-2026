namespace TodoApp.Desktop.Services;

public sealed class NavigationService
{
    private Action<object>? _navigate;

    public void Initialize(Action<object> navigate)
    {
        _navigate = navigate ?? throw new ArgumentNullException(nameof(navigate));
    }

    public void Navigate(object viewModel)
    {
        if (_navigate == null)
        {
            throw new InvalidOperationException("Navigation service is not initialized.");
        }

        _navigate(viewModel);
    }
}
