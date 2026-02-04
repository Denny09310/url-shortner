namespace System.Collections.Generic;

internal static class CollectionExtensions
{
    internal static IEnumerable<T> ReplaceWhere<T>(
        this IEnumerable<T> source,
        Func<T, bool> predicate,
        Func<T, T> replacement)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(replacement);

        return source.Select(x => predicate(x) ? replacement(x) : x);
    }

    internal static List<T> ReplaceWhere<T>(
        this List<T> source,
        Func<T, bool> predicate,
        Func<T, T> replacement)
    {
        for (var i = 0; i < source.Count; i++)
        {
            if (predicate(source[i]))
                source[i] = replacement(source[i]);
        }

        return source;
    }
}

