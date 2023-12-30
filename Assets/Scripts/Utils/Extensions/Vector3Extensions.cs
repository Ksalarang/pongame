using UnityEngine;

namespace Utils.Extensions {
public static class Vector3Extensions {

    public static bool approximately(this Vector3 vector, Vector3 other, bool ignoreZ = true, float epsilon = 0.001f) {
        if (Mathf.Abs(vector.x - other.x) > epsilon) return false;
        if (Mathf.Abs(vector.y - vector.y) > epsilon) return false;
        if (ignoreZ) return true;
        return Mathf.Abs(vector.z - other.z) < epsilon;
    }

    public static float distanceTo(this Vector3 vector, Vector3 other, bool ignoreZ = true) {
        var deltaX = vector.x - other.x;
        var deltaY = vector.y - other.y;
        if (ignoreZ) return Mathf.Sqrt(deltaX * deltaX + deltaY * deltaY);
        var deltaZ = vector.z - other.z;
        return Mathf.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ);
    }

    public static Vector3 midPoint(this Vector3 vector, Vector3 other) {
        return new Vector3(
            (vector.x + other.x) / 2,
            (vector.y + other.y) / 2,
            (vector.z + other.z) / 2
        );
    }

    /// <summary>
    /// Return a vector populated with the minimum value of this vector.
    /// </summary>
    public static Vector3 getMin(this Vector3 vector, bool includeZ = false) {
        var min = vector.minValue(includeZ);
        return new Vector3(min, min, includeZ ? min : vector.z);
    }
    
    public static float minValue(this Vector3 vector, bool includeZ = false) {
        return includeZ ? MathUtils.min(vector.x, vector.y, vector.z) : Mathf.Min(vector.x, vector.y);
    }

    public static Vector3 clamp(this Vector3 vector, Vector3 min, Vector3 max) {
        if (vector.x < min.x) vector.x = min.x;
        if (vector.x > max.x) vector.x = max.x;
        if (vector.y < min.y) vector.y = min.y;
        if (vector.y > max.y) vector.y = max.y;
        if (vector.z < min.z) vector.z = min.z;
        if (vector.z > max.z) vector.z = max.z;
        return vector;
    }
}
}