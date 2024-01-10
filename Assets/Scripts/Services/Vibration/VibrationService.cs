using System;
using services;
using services.saves;
using UnityEngine;
using Utils;
using Zenject;

namespace Services.Vibration {
public class VibrationService : Service, SaveLoadListener {

#if UNITY_ANDROID && !UNITY_EDITOR
    static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    static AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    static AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
    static readonly bool isAndroid = true;
#else
    static AndroidJavaClass unityPlayer;
    static AndroidJavaObject currentActivity;
    static AndroidJavaObject vibrator;
    static readonly bool isAndroid = false;
#endif
    
    readonly Log log;
    AudioSave save;

    [Inject]
    public VibrationService() {
        log = new(GetType(), false);
        log.log("create");
    }

    public void onSaveLoaded(PlayerSave playerSave) {
        save = playerSave.audio;
    }

    public void vibrate(VibrationType type) {
        if (!save.vibrationEnabled) return;
        log.log($"vibrate: {type}");
        vibrate(getVibrationDuration(type));
    }
    
    public void setVibrationEnabled(bool enabled) {
        save.vibrationEnabled = enabled;
    }

    public bool isVibrationEnabled() => save.vibrationEnabled;

    public bool supportsVibration() => SystemInfo.supportsVibration;

    void vibrate(long milliseconds) {
        if (isAndroid) vibrator.Call("vibrate", milliseconds);
        // else Handheld.Vibrate();
    }

    void vibrate(long[] pattern, int repeat) {
        if (isAndroid) vibrator.Call("vibrate", pattern, repeat);
        // else Handheld.Vibrate();
    }

    void cancel() {
        if (isAndroid) vibrator.Call("cancel");
    }

    long getVibrationDuration(VibrationType type) {
        return type switch {
            VibrationType.Ms20 => 20,
            VibrationType.Ms30 => 30,
            VibrationType.Ms50 => 50,
            VibrationType.Ms100 => 100,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    void doNotCallThisMethod() {
        Handheld.Vibrate(); // for vibration permission
    }
}

public enum VibrationType {
    Ms20, Ms30, Ms50, Ms100,
}
}