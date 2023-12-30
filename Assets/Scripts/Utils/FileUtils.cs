using System;
using System.IO;
using UnityEngine;

namespace Utils {
public static class FileUtils {
    
    public static bool writeToFile(string fileName, string data) {
        var fullPath = Path.Combine(Application.persistentDataPath, fileName);
        try {
            File.WriteAllText(fullPath, data);
            return true;
        } catch (Exception e) {
            Log.warn("FileUtils", $"failed to write to '{fullPath}' with exception {e}");
            return false;
        }
    }

    public static bool loadFromFile(string fileName, out string data) {
        var fullPath = Path.Combine(Application.persistentDataPath, fileName);
        try {
            data = File.ReadAllText(fullPath);
            return true;
        }
        catch (Exception e) {
            Log.warn("FileUtils", $"Failed to read from '{fullPath}' with exception {e}");
            data = "";
            return false;
        }
    }
}
}