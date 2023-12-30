using UnityEngine;

namespace Utils.Extensions {
public static class CameraExtensions {
    public static Vector3 getBottomLeft(this Camera camera) {
        return camera.ScreenToWorldPoint(Vector3.zero);
    }

    public static Vector3 getTopLeft(this Camera camera) {
        return camera.ScreenToWorldPoint(new Vector3(0, Screen.height));
    }

    public static Vector3 getTopRight(this Camera camera) {
        return camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
    }

    public static Vector3 getBottomRight(this Camera camera) {
        return camera.ScreenToWorldPoint(new Vector3(Screen.width, 0));
    }

    public static float getScreenWidth(this Camera camera) {
        var leftX = camera.ScreenToWorldPoint(new Vector3(0, 0)).x;
        var rightX = camera.ScreenToWorldPoint(new Vector3(Screen.width, 0)).x;
        return rightX - leftX;
    }
}
}