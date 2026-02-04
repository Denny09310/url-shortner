using Microsoft.JSInterop;

namespace Client.Services;

internal sealed class Clipboard(IJSRuntime js)
{
    public ValueTask WriteTextAsync(string text)
        => js.InvokeVoidAsync("navigator.clipboard.writeText", text);
}
