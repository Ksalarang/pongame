using System;
using GameInstaller;
using GameScene;
using TMPro;
using UnityEngine;
using Utils;
using Utils.Extensions;
using Zenject;

namespace Controllers {
public class GameController : MonoBehaviour {
    [Inject] GameSettings settings;
    [Inject] new Camera camera;
    [Inject] BallController ballController;
    [Inject] InputController inputController;

    [Inject(Id = GameObjectId.TopBorder)] GameObject topBorder;
    [Inject(Id = GameObjectId.BottomBorder)] GameObject bottomBorder;
    [Inject(Id = LabelId.Label1)] TMP_Text player1Label;
    [Inject(Id = LabelId.Label2)] TMP_Text player2Label;

    Log log;
    int player1Score;
    int player2Score;
    
    public bool gamePaused { get; private set; }
    
    void Awake() {
        log = new Log(GetType());
        Application.targetFrameRate = 60;
    }

    void Start() {
        ballController.setLimits(
            camera.getBottomLeft().x,
            camera.getTopRight().x,
            topBorder.transform.localPosition.y,
            bottomBorder.transform.localPosition.y
        );
    }

    public void onPlayerOneScored() {
        if (gamePaused) return;
        log.log("on player one scored");
        player1Score++;
        player1Label.text = player1Score.ToString();
        onPlayerScored();
    }

    public void onPlayerTwoScored() {
        if (gamePaused) return;
        log.log("on player two scored");
        player2Score++;
        player2Label.text = player2Score.ToString();
        onPlayerScored();
    }

    void onPlayerScored() {
        gamePaused = inputController.paused = true;
        StartCoroutine(Coroutines.delayAction(settings.delayBeforeReset, () => {
            ballController.resetBall();
            gamePaused = inputController.paused = false;
        }));
    }
}
}