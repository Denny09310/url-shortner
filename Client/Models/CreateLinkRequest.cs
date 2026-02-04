namespace Shared.Models;

public sealed record CreateLinkRequest(
    string ShortCode,
    string TargetUrl
);
