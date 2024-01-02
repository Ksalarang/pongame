using System;
using GameInstaller;
using GameScene;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Utils.Extensions;
using Zenject;

namespace Controllers {
public class GameController : MonoBehaviour {
    [Inject] GameSettings settings;
    [Inject] new Camera camera;
    [Inject] BallController ballController;
    [Inject] InputController inputController;
    [Inject(Id = StickControllerId.Stick1)] StickController playerStick;
    [Inject(Id = StickControllerId.Stick2)] StickController botStick;

    [Inject(Id = GameObjectId.TopBorder)] GameObject topBorder;
    [Inject(Id = GameObjectId.BottomBorder)] GameObject bottomBorder;
    [Inject(Id = LabelId.Label1)] TMP_Text player1Label;
    [Inject(Id = LabelId.Label2)] TMP_Text player2Label;
    [Inject(Id = ViewId.AutoplayToggle)] Toggle autoplayToggle;

    Log log;
    int player1Score;
    int player2Score;
    
    public bool gamePaused { get; private set; }
    
    void Awake() {
        log = new Log(GetType());
        Application.targetFrameRate = settings.targetFrameRate;
        initAutoplayToggle();
    }

    void initAutoplayToggle() {
        autoplayToggle.isOn = settings.debug.autoPlay;
        autoplayToggle.onValueChanged.AddListener(value => {
            settings.debug.autoPlay = value;
        });
    }

    void Start() {
        ballController.setLimits(
            camera.getBottomLeft().x,
            camera.getTopRight().x,
            topBorder.transform.localPosition.y,
            bottomBorder.transform.localPosition.y
        );
    }

    void Update() {
        Time.timeScale = settings.timeScale;
    }

    public void onPlayerOneScored() {
        if (gamePaused) return;
        // log.log("on player one scored");
        player1Score++;
        player1Label.text = player1Score.ToString();
        onPlayerScored();
    }

    public void onPlayerTwoScored() {
        if (gamePaused) return;
        // log.log("on player two scored");
        player2Score++;
        player2Label.text = player2Score.ToString();
        onPlayerScored();
    }

    void onPlayerScored() {
        gamePaused = inputController.paused = true;
        StartCoroutine(Coroutines.delayAction(settings.delayBeforeReset, () => {
            ballController.resetBall();
            botStick.reset();
            playerStick.reset();
            gamePaused = inputController.paused = false;
        }));
    }
}
}