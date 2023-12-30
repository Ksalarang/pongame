using Controllers;
using GameInstaller;
using Models;
using TMPro;
using UnityEngine;
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
    [Header("Models")]
    [SerializeField] Ball ball;
    [SerializeField] GameObject topBorder;
    [SerializeField] GameObject bottomBorder;
    [Header("UI")]
    [SerializeField] TMP_Text scoreLabel1;
    [SerializeField] TMP_Text scoreLabel2;
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
        // models
        bind(ball);
        bind(topBorder, GameObjectId.TopBorder);
        bind(bottomBorder, GameObjectId.BottomBorder);
        // UI
        bind(scoreLabel1, LabelId.Label1);
        bind(scoreLabel2, LabelId.Label2);
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

public enum LabelId {
    Label1, Label2,
}
}