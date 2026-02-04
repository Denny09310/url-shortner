namespace Client.Services;

public abstract class StateContainer : IState
{
    public event Action? StateChanged;

    public void Notify() => StateChanged?.Invoke();

    protected void Set<T>(ref T field, T value)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return;

        field = value;
        StateChanged?.Invoke();
    }
}