using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Server.Data;

namespace Server.Endpoints;

[Tags("Redirect")]
[MapGroup("/redirect")]
internal static class RedirectEndpoints
{
    [MapGet("/{code}")]
    internal static async Task<Results<NotFound, RedirectHttpResult>> RedirectTo(
        string code,
        HybridCache cache,
        ApplicationDbContext db,
        CancellationToken ct)
    {
        code = code.Trim().ToLowerInvariant();

        var link = await cache.GetOrCreateAsync(
            key: $"link:{code}",
            async (ct) => await db.Links
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.ShortCode == code && l.IsActive, ct),
            cancellationToken: ct);

        return link is null
            ? TypedResults.NotFound()
            : TypedResults.Redirect(link.TargetUrl);
    }
}
