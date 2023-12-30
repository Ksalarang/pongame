using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Utils.Extensions {
[SuppressMessage("ReSharper", "SwapViaDeconstruction")]
public static class IListExtensions {
    public static void shuffle<T>(this IList<T> list) {
        var count = list.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i) {
            var r = Random.Range(i, count);
            var tmp = list[i];
            list[i] = list[r];
            list[r] = tmp;
        }
    }

    public static void shuffleModern<T>(this IList<T> list) {
        var last = list.Count - 1;
        while (last > 0) {
            var random = Random.Range(0, last);
            var temp = list[last];
            list[last] = list[random];
            list[random] = temp;
            last--;
        }
    }
}
}