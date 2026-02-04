using Shared.Models;

namespace Client.Services;

internal sealed class ManagementState(ApiClient api) : StateContainer
{
    private List<LinkDto>? _links;

    public IReadOnlyList<LinkDto>? Links => _links;

    public string? Search
    {
        get;
        set => Set(ref field, value);
    }

    public async Task<StateResult> CreateAsync(string shortCode, string targetUri)
    {
        var response = await api.Links.CreateAsync(
            new CreateLinkRequest(shortCode, targetUri));

        if (!response.IsSuccess)
            return StateResult.Failure;

        _links ??= [];
        _links.Insert(0, response.Value!);
        Notify();

        return StateResult.Success;
    }

    public async Task<StateResult> DeleteAsync(Guid id)
    {
        var response = await api.Links.DeleteAsync(id);

        if (!response.IsSuccess)
            return StateResult.Failure;

        _links?.RemoveAll(l => l.Id == id);
        Notify();

        return StateResult.Success;
    }

    public async Task<StateResult> EditAsync(Guid id, string targetUrl)
    {
        var response = await api.Links.UpdateAsync(
            id,
            new UpdateLinkRequest(targetUrl));

        if (!response.IsSuccess)
            return StateResult.Failure;

        _links = _links?
            .ReplaceWhere(
                l => l.Id == id,
                l => l with { TargetUrl = targetUrl });

        Notify();

        return StateResult.Success;
    }

    public async Task<StateResult> PopulateAsync()
    {
        var response = await api.Links.GetAsync(
            new GetLinksRequest(Search));

        if (!response.IsSuccess)
            return StateResult.Failure;

        _links = response.Value?.ToList();
        Notify();

        return StateResult.Success;
    }
}

public enum StateResult
{
    Success,
    Failure
}