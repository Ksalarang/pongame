using UnityEngine;

namespace Utils.Extensions {
public static class RectTransformExtensions {
    // temp variable to retrieve world corners' coordinates which are stored starting from the bottom left corner clock-wise
    static readonly Vector3[] corners = new Vector3[4];

    public static Rect getWorldRect(this RectTransform rectTransform, float canvasScale = 1f) {
        rectTransform.GetWorldCorners(corners);
        for (var i = 0; i < corners.Length; i++) corners[i].z = 0;
        var size = new Vector2(
            corners[3].x - corners[0].x,
            corners[1].y - corners[0].y
        );
        return new Rect(corners[0] * canvasScale, size * canvasScale);
    }
}
}