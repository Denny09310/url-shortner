namespace Client.Services;

public interface IState
{
    event Action? StateChanged;
}
