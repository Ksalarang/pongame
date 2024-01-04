using UnityEngine;

namespace Utils {
public static class MathUtils {
    public static float min(float a, float b, float c) {
        var min = Mathf.Min(a, b);
        return min < c ? min : c;
    }

    /// <summary>
    /// Ensures that the value is not less than the specified min value.
    /// </summary>
    public static float coerceAtLeast(float value, float min) => Mathf.Max(min, value);

    /// <summary>
    /// Ensures that the value is not greater than the specified max value.
    /// </summary>
    public static float coerceAtMost(float value, float max) => Mathf.Min(value, max);
    
    /// <summary>
    /// Ensures that the value is not less than the specified min value.
    /// </summary>
    public static int coerceAtLeast(int value, int min) => Mathf.Max(min, value);

    /// <summary>
    /// Ensures that the value is not greater than the specified max value.
    /// </summary>
    public static int coerceAtMost(int value, int max) => Mathf.Min(value, max);

    public static float vector2ToAngle(Vector2 direction) => Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
}
}