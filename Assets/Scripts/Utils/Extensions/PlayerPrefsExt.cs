using UnityEngine;

namespace Utils.Extensions {
public static class PlayerPrefsExt {
    public static void setBool(string key, bool value) {
        var v = value ? 1 : 0;
        PlayerPrefs.SetInt(key, v);
    }
    
    public static bool getBool(string key, bool defaultValue = false) {
        var dv = defaultValue ? 1 : 0;
        var value = PlayerPrefs.GetInt(key, dv);
        return value == 1;
    }
}
}