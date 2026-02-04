namespace Client.Services;

internal sealed record ApiResult<T>(
    bool IsSuccess,
    T? Value,
    string? Error)
{
    public static ApiResult<T> Success(T value) => new(true, value, null);
    public static ApiResult<T> Fail(string error) => new(false, default, error);
}

internal sealed record ApiResult(
    bool IsSuccess,
    string? Error)
{
    public static ApiResult Success() => new(true, null);
    public static ApiResult Fail(string error) => new(false, error);
}
