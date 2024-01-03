using System;
using GameInstaller;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Windows {
public class WinScoreWindow : Window {
    [SerializeField] Button tenScoreButton;
    [SerializeField] Button twentyScoreButton;
    [SerializeField] Button unlimitedScoreButton;

    [Inject] GameSettings gameSettings;

    void Awake() {
        tenScoreButton.onClick.AddListener(() => {
            onClickScoreButton(10);
        });
        twentyScoreButton.onClick.AddListener(() => {
            onClickScoreButton(20);
        });
        unlimitedScoreButton.onClick.AddListener(() => {
            onClickScoreButton(int.MaxValue);
        });
    }

    void onClickScoreButton(int scores) {
        gameSettings.winPoints = scores;
        hide();
    }
}
}