using System;
using GameInstaller;
using ModestTree;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Controllers {
public class DifficultyPicker : MonoBehaviour {
    [SerializeField] TMP_Text label;
    [SerializeField] Button leftButton;
    [SerializeField] Button rightButton;

    [Inject] GameSettings gameSettings;

    DifficultySettings.Mode[] modeEnums;
    int currentIndex;

    void Awake() {
        modeEnums = (DifficultySettings.Mode[]) Enum.GetValues(typeof(DifficultySettings.Mode));
        currentIndex = modeEnums.IndexOf(gameSettings.difficultyMode);
        
        leftButton.onClick.AddListener(() => onButtonPressed(-1));
        rightButton.onClick.AddListener(() => onButtonPressed(1));
        
        setLabelText(modeEnums[currentIndex]);
    }

    void onButtonPressed(int direction) {
        var index = currentIndex + direction;
        if (index < 0 || modeEnums.Length <= index) {
            return;
        }
        currentIndex = index;
        gameSettings.difficultyMode = modeEnums[currentIndex];
        setLabelText(gameSettings.difficultyMode);
    }

    void setLabelText(DifficultySettings.Mode mode) {
        label.text = mode.ToString();
    }
}
}