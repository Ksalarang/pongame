using UnityEngine;

namespace Utils.Extensions {
public static class ColorExtensions {
    public static bool approximately(this Color color, Color other, bool ignoreAlpha = false, float epsilon = 0.001f) {
        if (Mathf.Abs(color.r - other.r) > epsilon) return false;
        if (Mathf.Abs(color.g - other.g) > epsilon) return false;
        if (Mathf.Abs(color.b - other.b) > epsilon) return false;
        if (ignoreAlpha) return true;
        return Mathf.Abs(color.a - other.a) < epsilon;
    } 
}
}