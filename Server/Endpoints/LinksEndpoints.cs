using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Data.Entities;
using Shared.Models;

namespace Server.Endpoints;


[Tags("Links")]
[MapGroup("/links")]
internal sealed class LinksEndpoints(ApplicationDbContext db)
{
    [MapGet("/")]
    internal async Task<Ok<List<LinkDto>>> GetLinks(
        [AsParameters] GetLinksRequest request,
        CancellationToken ct)
    {
        var query = db.Links
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Query))
        {
            query = query
                .Where(l => l.ShortCode.Contains(request.Query)
                         || l.TargetUrl.Contains(request.Query));
        }

        var links = await query
            .OrderByDescending(l => l.CreatedAt)
            .Select(l => new LinkDto(
                l.Id,
                l.ShortCode,
                l.TargetUrl,
                l.IsActive,
                l.CreatedAt,
                l.LastUpdatedAt))
            .ToListAsync(ct);

        return TypedResults.Ok(links);
    }

    [MapGet("/{id:guid}")]
    internal async Task<Results<NotFound, Ok<LinkDto>>> GetLink(
        Guid id,
        CancellationToken ct)
    {
        var link = await db.Links
            .AsNoTracking()
            .Where(l => l.Id == id)
            .Select(l => new LinkDto(
                l.Id,
                l.ShortCode,
                l.TargetUrl,
                l.IsActive,
                l.CreatedAt,
                l.LastUpdatedAt))
            .FirstOrDefaultAsync(ct);

        return link is null
            ? TypedResults.NotFound()
            : TypedResults.Ok(link);
    }

    [MapPost("/")]
    internal async Task<Results<BadRequest<string>, Conflict<string>, Created<LinkDto>>> CreateLink(
        CreateLinkRequest request,
        CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.ShortCode))
            return TypedResults.BadRequest("ShortCode is required.");

        if (!Uri.TryCreate(request.TargetUrl, UriKind.Absolute, out _))
            return TypedResults.BadRequest("Invalid target URL.");

        var shortCode = request.ShortCode.Trim().ToLowerInvariant();

        var exists = await db.Links
            .AnyAsync(l => l.ShortCode == shortCode, ct);

        if (exists)
            return TypedResults.Conflict("ShortCode already exists.");

        var link = new Link(shortCode, request.TargetUrl);

        db.Links.Add(link);
        await db.SaveChangesAsync(ct);

        var dto = new LinkDto(
            link.Id,
            link.ShortCode,
            link.TargetUrl,
            link.IsActive,
            link.CreatedAt,
            link.LastUpdatedAt);

        return TypedResults.Created($"/links/{link.Id}", dto);
    }

    [MapPut("/{id:guid}")]
    internal async Task<Results<NotFound, BadRequest<string>, NoContent>> UpdateLink(
        Guid id,
        UpdateLinkRequest request,
        CancellationToken ct)
    {
        if (!Uri.TryCreate(request.TargetUrl, UriKind.Absolute, out _))
            return TypedResults.BadRequest("Invalid target URL.");

        var link = await db.Links
            .FirstOrDefaultAsync(l => l.Id == id, ct);

        if (link is null)
            return TypedResults.NotFound();

        link.Edit(request.TargetUrl);

        await db.SaveChangesAsync(ct);

        return TypedResults.NoContent();
    }

    [MapPost("/{id:guid}/activate")]
    internal async Task<Results<NotFound, NoContent>> ActivateLink(
        Guid id,
        CancellationToken ct)
    {
        var link = await db.Links.FirstOrDefaultAsync(l => l.Id == id, ct);

        if (link is null)
            return TypedResults.NotFound();

        link.Activate();
        await db.SaveChangesAsync(ct);

        return TypedResults.NoContent();
    }

    [MapPost("/{id:guid}/deactivate")]
    internal async Task<Results<NotFound, NoContent>> DeactivateLink(
        Guid id,
        CancellationToken ct)
    {
        var link = await db.Links.FirstOrDefaultAsync(l => l.Id == id, ct);

        if (link is null)
            return TypedResults.NotFound();

        link.Deactivate();
        await db.SaveChangesAsync(ct);

        return TypedResults.NoContent();
    }

    [MapDelete("/{id:guid}")]
    internal async Task<Results<NotFound, NoContent>> DeleteLink(
        Guid id,
        CancellationToken ct)
    {
        var link = await db.Links.FirstOrDefaultAsync(l => l.Id == id, ct);

        if (link is null)
            return TypedResults.NotFound();

        db.Links.Remove(link);
        await db.SaveChangesAsync(ct);

        return TypedResults.NoContent();
    }
}