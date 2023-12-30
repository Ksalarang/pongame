using UnityEngine;

namespace Utils.Extensions {
public static class TransformExtensions {
    public static Vector2 getBottomLeft(this Transform transform) {
        var position = transform.position;
        var scale = transform.localScale;
        return new Vector2(
            position.x - scale.x / 2,
            position.y - scale.y / 2
        );
    }

    public static float getTopY(this Transform transform) {
        var position = transform.position;
        var scale = transform.lossyScale;
        return position.y + scale.y / 2;
    }

    public static void setX(this Transform transform, float x) {
        var position = transform.position;
        transform.position = new Vector3(x, position.y, position.z);;
    }

    public static void setY(this Transform transform, float y) {
        var position = transform.position;
        transform.position = new Vector3(position.x, y, position.z);
    }
    
    public static void setZ(this Transform transform, float z) {
        var position = transform.position;
        transform.position = new Vector3(position.x, position.y, z);
    }

    public static void setLocalZ(this Transform transform, float z) {
        var position = transform.localPosition;
        transform.localPosition = new Vector3(position.x, position.y, z);
    }

    public static float getMinX(this Transform transform) => transform.position.x - transform.localScale.x / 2;
    
    public static float getMaxX(this Transform transform) => transform.position.x + transform.localScale.x / 2;

    public static void fitIn(this Transform transform, Transform other) {
        var otherScale = other.localScale;
        var scale = Mathf.Min(otherScale.x, otherScale.y);
        transform.localScale = new Vector3(scale, scale);
    }

    public static Vector3 getMinScale(this Transform transform) {
        var localScale = transform.localScale;
        var min = Mathf.Min(localScale.x, localScale.y);
        return new Vector3(min, min);
    }

    /// <summary>
    /// Returns the angle in degrees between two objects in range [-180, 180].
    /// If the other object is to the left of the first object,
    /// then the angle is positive and vice versa. 
    /// </summary>
    public static float angleDegrees(this Transform transform, Transform other) {
        var direction = other.transform.position - transform.position;
        direction = other.transform.InverseTransformDirection(direction);
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }
}
}