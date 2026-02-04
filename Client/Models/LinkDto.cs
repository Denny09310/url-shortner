namespace Shared.Models;

public sealed record LinkDto(
    Guid Id,
    string ShortCode,
    string TargetUrl,
    bool IsActive,
    DateTime CreatedAt,
    DateTime LastUpdatedAt
);
