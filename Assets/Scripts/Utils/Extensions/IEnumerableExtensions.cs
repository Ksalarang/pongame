using System.Collections.Generic;

namespace Utils.Extensions {
public static class IEnumerableExtensions {
    public static string toString<T>(this IEnumerable<T> enumerable) => string.Join(", ", enumerable);
}
}