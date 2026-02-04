using System.Net.Http.Json;
using Shared.Models;

namespace Client.Services;

internal sealed class LinksEndpoints(HttpClient http, string route = "/links")
{
    public async Task<ApiResult<IReadOnlyList<LinkDto>>> GetAsync(
        GetLinksRequest request,
        CancellationToken ct = default)
    {
        try
        {
            var links = await http.GetFromJsonAsync<IReadOnlyList<LinkDto>>(
                $"{route}{request.ToQueryString()}", ct);

            return ApiResult<IReadOnlyList<LinkDto>>
                .Success(links ?? []);
        }
        catch (Exception ex)
        {
            return ApiResult<IReadOnlyList<LinkDto>>
                .Fail(ex.Message);
        }
    }

    public async Task<ApiResult<LinkDto>> GetAsync(
        Guid id,
        CancellationToken ct = default)
    {
        var response = await http.GetAsync($"{route}/{id}", ct);

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return ApiResult<LinkDto>.Fail("Link not found.");

        if (!response.IsSuccessStatusCode)
            return ApiResult<LinkDto>.Fail("Failed to retrieve link.");

        var dto = await response.Content.ReadFromJsonAsync<LinkDto>(ct);

        return dto is null
            ? ApiResult<LinkDto>.Fail("Invalid response.")
            : ApiResult<LinkDto>.Success(dto);
    }

    public async Task<ApiResult<LinkDto>> CreateAsync(
        CreateLinkRequest request,
        CancellationToken ct = default)
    {
        var response = await http.PostAsJsonAsync(
            route,
            request,
            ct);

        if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
            return ApiResult<LinkDto>.Fail("Short code already exists.");

        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            var error = await response.Content.ReadAsStringAsync(ct);
            return ApiResult<LinkDto>.Fail(error);
        }

        if (!response.IsSuccessStatusCode)
            return ApiResult<LinkDto>.Fail("Failed to create link.");

        var dto = await response.Content.ReadFromJsonAsync<LinkDto>(ct);

        return dto is null
            ? ApiResult<LinkDto>.Fail("Invalid response.")
            : ApiResult<LinkDto>.Success(dto);
    }

    public async Task<ApiResult> UpdateAsync(
        Guid id,
        UpdateLinkRequest request,
        CancellationToken ct = default)
    {
        var response = await http.PutAsJsonAsync(
            $"{route}/{id}",
            request,
            ct);

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return ApiResult.Fail("Link not found.");

        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            var error = await response.Content.ReadAsStringAsync(ct);
            return ApiResult.Fail(error);
        }

        return response.IsSuccessStatusCode
            ? ApiResult.Success()
            : ApiResult.Fail("Failed to update link.");
    }

    public async Task<ApiResult> ActivateAsync(
        Guid id,
        CancellationToken ct = default)
    {
        var response = await http.PostAsync(
            $"{route}/{id}/activate",
            content: null,
            ct);

        return response.StatusCode == System.Net.HttpStatusCode.NotFound
            ? ApiResult.Fail("Link not found.")
            : response.IsSuccessStatusCode
                ? ApiResult.Success()
                : ApiResult.Fail("Failed to activate link.");
    }

    public async Task<ApiResult> DeactivateAsync(
        Guid id,
        CancellationToken ct = default)
    {
        var response = await http.PostAsync(
            $"{route}/{id}/deactivate",
            content: null,
            ct);

        return response.StatusCode == System.Net.HttpStatusCode.NotFound
            ? ApiResult.Fail("Link not found.")
            : response.IsSuccessStatusCode
                ? ApiResult.Success()
                : ApiResult.Fail("Failed to deactivate link.");
    }

    public async Task<ApiResult> DeleteAsync(
        Guid id,
        CancellationToken ct = default)
    {
        var response = await http.DeleteAsync(
            $"{route}/{id}",
            ct);

        return response.StatusCode == System.Net.HttpStatusCode.NotFound
            ? ApiResult.Fail("Link not found.")
            : response.IsSuccessStatusCode
                ? ApiResult.Success()
                : ApiResult.Fail("Failed to delete link.");
    }
}
