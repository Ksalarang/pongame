using UnityEngine;

namespace Utils {
public static class MathUtils {
    public static float min(float a, float b, float c) {
        var min = Mathf.Min(a, b);
        return min < c ? min : c;
    }

    public static float vector2ToAngle(Vector2 direction) => Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
}
}