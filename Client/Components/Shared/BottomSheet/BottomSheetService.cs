using Microsoft.AspNetCore.Components;

namespace Client.Services;

public class BottomSheetService
{
    public event Func<Task>? CloseRequestedAsync;

    public event Func<RenderFragment, Task>? OpenRequestedAsync;

    public async Task CloseAsync()
    {
        if (CloseRequestedAsync is null)
        {
            return;
        }

        await CloseRequestedAsync.Invoke();
    }

    public async Task OpenAsync<T>(Dictionary<string, object>? parameters = null) where T : IComponent
    {
        if (OpenRequestedAsync is null)
        {
            return;
        }

        var body = Wrap(new RenderFragment(builder =>
        {
            int seq = 0;
            builder.OpenComponent<T>(seq++);
            builder.AddMultipleAttributes(seq++, parameters);
            builder.CloseComponent();
        }));

        await OpenRequestedAsync.Invoke(body);
    }

    private RenderFragment Wrap(RenderFragment fragment) => builder =>
    {
        var instance = new BottomSheetInstance(this);

        int seq = 0;
        builder.OpenComponent<CascadingValue<BottomSheetInstance>>(seq++);
        builder.AddComponentParameter(seq++, nameof(CascadingValue<>.Value), instance);
        builder.AddComponentParameter(seq++, nameof(CascadingValue<>.IsFixed), true);
        builder.AddComponentParameter(seq++, nameof(CascadingValue<>.ChildContent), fragment);
        builder.CloseComponent();
    };
}

public sealed class BottomSheetInstance(BottomSheetService service)
{
    public Task CloseAsync() => service.CloseAsync();
}