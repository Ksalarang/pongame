using System;
using System.Collections;
using UnityEngine;
using Utils.Interfaces;

namespace Utils {
public static class Coroutines {
    const float MinDuration = 0.001f;
    
    //todo: check duration for 0;
    public static IEnumerator moveTo(Transform transform, Vector3 end, float duration, 
        Interpolation interpolation = Interpolation.Linear, Action action = null) {
        duration = Mathf.Max(MinDuration, duration);
        var start = transform.position;
        var time = 0f;
        while (time < duration) {
            time += Time.deltaTime;
            var t = interpolate(time / duration, interpolation);
            transform.position = Vector3.Lerp(start, end, t);
            yield return null;
        }
        action?.Invoke();
    }
    
    public static IEnumerator moveTo(Transform transform, Vector3 middle, Vector3 end, float duration, Action action = null) {
        duration = Mathf.Max(MinDuration, duration);
        var start = transform.position;
        var time = 0f;
        var halfDuration = duration / 2;
        while (time < halfDuration) {
            time += Time.deltaTime;
            transform.position = Vector3.Lerp(start, middle, time / halfDuration);
            yield return null;
        }
        time = 0f;
        while (time < halfDuration) {
            time += Time.deltaTime;
            transform.position = Vector3.Lerp(middle, end, time / halfDuration);
            yield return null;
        }
        action?.Invoke();
    }

    public static IEnumerator scaleTo(Transform transform, Vector3 end, float duration, Action action = null) {
        duration = Mathf.Max(MinDuration, duration);
        var start = transform.localScale;
        var time = 0f;
        while (time < duration) {
            time += Time.deltaTime;
            transform.localScale = Vector3.Lerp(start, end, time / duration);
            yield return null;
        }
        action?.Invoke();
    }

    public static IEnumerator scaleToAndBack(Transform transform, Vector3 end, float duration, 
        bool isLinear = true, Action action = null) {
        duration = Mathf.Max(MinDuration, duration);
        var start = transform.localScale;
        var time = 0f;
        var halfDuration = duration / 2;
        var interpolation = isLinear ? Interpolation.Linear : Interpolation.Accelerate;
        while (time < halfDuration) {
            time += Time.deltaTime;
            transform.localScale = Vector3.Lerp(start, end, interpolate(time / halfDuration, interpolation));
            yield return null;
        }
        time = 0f;
        interpolation = isLinear ? Interpolation.Linear : Interpolation.Decelerate;
        while (time < halfDuration) {
            time += Time.deltaTime;
            transform.localScale = Vector3.Lerp(end, start, interpolate(time / halfDuration, interpolation));
            yield return null;
        }
        action?.Invoke();
    }
    
    public static IEnumerator fadeTo(Colorable colorable, float alpha, float duration, Action action = null) {
        duration = Mathf.Max(MinDuration, duration);
        var start = colorable.color;
        var end = new Color(start.r, start.g, start.b, alpha);
        var time = 0f;
        while (time < duration) {
            time += Time.deltaTime;
            colorable.color = Color.Lerp(start, end, time / duration);
            yield return null;
        }
        action?.Invoke();
    }

    public static IEnumerator fadeTo(AlphaAdjustable alphaAdjustable, float alpha, float duration, Action action = null) {
        duration = Mathf.Max(MinDuration, duration);
        var start = alphaAdjustable.alpha;
        var time = 0f;
        while (time < duration) {
            time += Time.deltaTime;
            alphaAdjustable.alpha = Mathf.Lerp(start, alpha, time / duration);
            yield return null;
        }
        action?.Invoke();
    }

    public static IEnumerator rotateAroundYAxis(Transform transform, bool left, float angle, float duration, Action action = null) {
        duration = Mathf.Max(MinDuration, duration);
        var start = transform.eulerAngles.y;
        var end = left ? start - angle : start + angle;
        var time = 0f;
        while (time < duration) {
            time += Time.deltaTime;
            var yAngle = Mathf.LerpAngle(start, end, time / duration);
            transform.rotation = Quaternion.Euler(0, yAngle, 0);
            yield return null;
        }
        action?.Invoke();
    }

    static float interpolate(float value, Interpolation interpolation) {
        return interpolation switch {
            Interpolation.Linear => value,
            Interpolation.Accelerate => Mathf.Pow(value, 2),
            Interpolation.AccelerateDecelerate => Mathf.SmoothStep(0f, 1f, value),
            Interpolation.Decelerate => Mathf.Pow(value, 0.5f),
            _ => throw new ArgumentOutOfRangeException(nameof(interpolation), interpolation, null)
        };
    }

    public static IEnumerator delayAction(float delay, Action action) {
        yield return new WaitForSeconds(delay);
        action.Invoke();
    }
}

public enum Interpolation {
    Linear,
    Accelerate,
    AccelerateDecelerate,
    Decelerate,
}
}