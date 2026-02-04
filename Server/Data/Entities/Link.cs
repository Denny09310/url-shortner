namespace Server.Data.Entities;

internal sealed class Link
{
    public Guid Id { get; private set; } = Guid.CreateVersion7();

    public string ShortCode { get; private set; } = null!;
    public string TargetUrl { get; private set; } = null!;

    public bool IsActive { get; private set; }

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime LastUpdatedAt { get; private set; } = DateTime.UtcNow;

    private Link() { } // EF Core

    public Link(string shortCode, string targetUrl)
    {
        ShortCode = NormalizeShortCode(shortCode);
        TargetUrl = ValidateUrl(targetUrl);

        IsActive = true;
    }

    public void Activate()
    {
        IsActive = true;
        Touch();
    }

    public void Deactivate()
    {
        IsActive = false;
        Touch();
    }

    public void Edit(string targetUrl)
    {
        TargetUrl = ValidateUrl(targetUrl);
        Touch();
    }

    private void Touch()
    {
        LastUpdatedAt = DateTime.UtcNow;
    }

    private static string NormalizeShortCode(string value)
        => value.Trim().ToLowerInvariant();

    private static string ValidateUrl(string value)
    {
        if (!Uri.TryCreate(value, UriKind.Absolute, out _))
            throw new ArgumentException("Invalid URL.", nameof(value));

        return value.Trim();
    }
}
