using System;
using System.Collections.Generic;
using GameInstaller;
using ModestTree;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Zenject;

namespace Windows {
public class SettingsWindow : Window {
    [SerializeField] Button closeButton;
    [Header("Win points section")]
    [SerializeField] SelectionButton winPointButton1;
    [SerializeField] SelectionButton winPointButton2;
    [SerializeField] SelectionButton winPointButton3;
    [SerializeField] SelectionButton winPointButton4;

    [Inject] GameSettings gameSettings;
    UISettings uiSettings;

    SelectionButton[] winPointButtons;

    void Awake() {
        uiSettings = gameSettings.uiSettings;
        
        closeButton.onClick.AddListener(() => hide());
        initializeWinPointButtons();
        
        applyWinPointsSettings();
    }

    void initializeWinPointButtons() {
        winPointButtons = new[] { winPointButton1, winPointButton2, winPointButton3, winPointButton4 };
        for (var i = 0; i < winPointButtons.Length; i++) {
            var button = winPointButtons[i];
            var points = gameSettings.winPointList[i];
            var text = points > 0 ? points.ToString() : "∞";
            button.setText(text);
            button.onClick = onClickWinPointButton;
        }
    }
    
    void onClickWinPointButton(SelectionButton clickedButton) {
        gameSettings.winPoints = getWinPointsForButton(clickedButton);
        var selectedColor = uiSettings.selectedButtonColor;
        var unselectedColor = uiSettings.unselectedButtonColor;
        foreach (var button in winPointButtons) {
            button.setSelected(button == clickedButton, selectedColor, unselectedColor);
        }
    }

    int getWinPointsForButton(SelectionButton button) {
        var index = winPointButtons.IndexOf(button);
        return gameSettings.winPointList[index];
    }

    void applyWinPointsSettings() {
        var index = gameSettings.winPointList.IndexOf(gameSettings.winPoints);
        index = MathUtils.coerceAtLeast(index, 0);
        onClickWinPointButton(winPointButtons[index]);
    }
}
}