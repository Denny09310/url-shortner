namespace Client.Services;

internal sealed class ApiClient(HttpClient http)
{
    public LinksEndpoints Links { get; } = new(http);
}