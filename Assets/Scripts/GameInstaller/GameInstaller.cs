using Windows;
using Controllers;
using GameInstaller;
using Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

// ReSharper disable All

namespace GameScene {
public class GameInstaller : MonoInstaller {
    [Header("Controllers")]
    [SerializeField] GameController gameController;
    [SerializeField] BallController ballController;
    [SerializeField] InputController inputController;
    [SerializeField] StickController stickController1;
    [SerializeField] StickController stickController2;
    [Header("Windows")]
    [SerializeField] SettingsWindow settingsWindow;
    [Header("Models")]
    [SerializeField] Ball ball;
    [SerializeField] GameObject topBorder;
    [SerializeField] GameObject bottomBorder;
    [Header("UI")]
    [SerializeField] Toggle autoplayToggle;
    [SerializeField] TMP_Text scoreLabel1;
    [SerializeField] TMP_Text scoreLabel2;
    [SerializeField] TMP_Text gameResultLabel;
    [Header("Misc")]
    [SerializeField] new Camera camera;
    [SerializeField] GameSettings gameSettings;

    public override void InstallBindings() {
        // controllers
        bind(gameController);
        bind(ballController);
        bind(inputController);
        bind(stickController1, StickControllerId.Stick1);
        bind(stickController2, StickControllerId.Stick2);
        // windows
        bind(settingsWindow);
        // models
        bind(ball);
        bind(topBorder, GameObjectId.TopBorder);
        bind(bottomBorder, GameObjectId.BottomBorder);
        // UI
        bind(autoplayToggle, UIElementId.AutoplayToggle);
        bind(scoreLabel1, UIElementId.PlayerOneScoreLabel);
        bind(scoreLabel2, UIElementId.PlayerTwoScoreLabel);
        bind(gameResultLabel, UIElementId.GameResultLabel);
        // misc
        bind(camera);
        bind(gameSettings);
    }
    
    void bind<T>(T instance) {
        Container.BindInstance(instance);
    }

    void bind<T>(T instance, object id) {
        Container.Bind<T>().WithId(id).FromInstance(instance);
    }
}

public enum StickControllerId {
    Stick1, Stick2,
}

public enum GameObjectId {
    TopBorder,
    BottomBorder,
}

public enum UIElementId {
    AutoplayToggle,
    GameResultLabel,
    PlayerOneScoreLabel,
    PlayerTwoScoreLabel,
}
}