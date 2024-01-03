using System;
using UnityEngine;
using Utils;

namespace GameInstaller {
[CreateAssetMenu(fileName = "GameSettings", menuName = "GameSettings")]
public class GameSettings : ScriptableObject {
    [Range(0f, 2f)] public float timeScale;
    public int targetFrameRate = 60;
    public float stickMaxSpeed = 10;
    public float delayBeforeReset = 0.5f;
    public float resultLabelDuration = 1f;

    public Color redStickColor;
    public Color blueStickColor;
    public Color ballColor;

    public int winPoints = 10;
    public DifficultySettings.Mode difficultyMode;
    public Difficulties difficulties;
    
    public LogSettings log;
    public DebugSettings debug;

    public DifficultySettings getDifficultySettings() {
        return difficultyMode switch {
            DifficultySettings.Mode.Easy => difficulties.easy,
            DifficultySettings.Mode.Medium => difficulties.medium,
            DifficultySettings.Mode.Hard => difficulties.hard,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}

[Serializable]
public class Difficulties {
    public DifficultySettings easy;
    public DifficultySettings medium;
    public DifficultySettings hard;
}

[Serializable]
public class DifficultySettings {
    public Mode mode;
    public float botMaxSpeed;
    public FloatRange botAccuracy;
    public int maxTrajectoryLines;
    public float ballInitialSpeed;
    
    public enum Mode {
        Easy, Medium, Hard
    }
}

[Serializable]
public class DebugSettings {
    public bool showBotPredictedPath;
    public bool autoPlay;
}

[Serializable]
public class LogSettings {
    public bool inputController;
}
}