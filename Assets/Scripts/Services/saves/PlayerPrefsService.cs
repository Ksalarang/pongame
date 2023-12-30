using UnityEngine;
using Utils;
using Utils.Extensions;
using utils.interfaces;
using Zenject;

namespace services.saves {
public class PlayerPrefsService : Service, AppLifecycleListener {
    readonly Log log;
    readonly PlayerPrefsData prefs;

    [Inject]
    public PlayerPrefsService() {
        log = new(GetType(), false);
        prefs = new PlayerPrefsData {
            audio = new AudioPrefs {
                soundVolume = PlayerPrefs.GetFloat(Keys.SoundVolume, 1f),
                musicVolume = PlayerPrefs.GetFloat(Keys.MusicVolume, 1f),
            },
            vibrationEnabled = PlayerPrefsExt.getBool(Keys.VibrationEnabled, true),
        };
        log.log($"init");
    }

    public PlayerPrefsData getPrefs() => prefs;

    public void onPause() {
        savePrefs();
    }

    public void onQuit() {
        savePrefs();
    }

    void savePrefs() {
        PlayerPrefs.SetFloat(Keys.SoundVolume, prefs.audio.soundVolume);
        PlayerPrefs.SetFloat(Keys.MusicVolume, prefs.audio.musicVolume);
        PlayerPrefsExt.setBool(Keys.VibrationEnabled, prefs.vibrationEnabled);
        PlayerPrefs.Save();
        log.log("save prefs");
    }

    static class Keys {
        public const string SoundVolume = "soundVolume";
        public const string MusicVolume = "musicVolume";
        public const string VibrationEnabled = "vibrationEnabled";
    }
}

public class PlayerPrefsData {
    public AudioPrefs audio;
    public bool vibrationEnabled;
}

public class AudioPrefs {
    public float soundVolume;
    public float musicVolume;
}
}