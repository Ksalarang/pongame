using UnityEngine;

namespace Utils.Extensions {
public static class Vector2Extensions {
    public static Vector2 getMidPoint(this Vector2 vector, Vector2 other) {
        return new Vector2(
            (vector.x + other.x) / 2,
            (vector.y + other.y) / 2
        );
    }
}
}