using System.Web;

namespace Shared.Models;

public sealed record GetLinksRequest(string? Query)
{
    public string ToQueryString()
    {
        var parameters = HttpUtility.ParseQueryString("");

        if (!string.IsNullOrWhiteSpace(Query))
            parameters.Add("query", Query);

        return parameters.Count == 0
            ? string.Empty
            : $"?{parameters}";
    }
}