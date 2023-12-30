using System;
using UnityEngine;

namespace GameInstaller {
[CreateAssetMenu(fileName = "GameSettings", menuName = "GameSettings")]
public class GameSettings : ScriptableObject {
    public float stickMaxSpeed = 5;
    public float delayBeforeReset = 0.5f;
    public LogConfig log;
}

[Serializable]
public class LogConfig {
    public bool inputController;
}
}