using System;
using Windows;
using GameInstaller;
using GameScene;
using Models;
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
    [Inject] Ball ball;
    [Inject] WinScoreWindow winScoreWindow;

    [Inject(Id = GameObjectId.TopBorder)] GameObject topBorder;
    [Inject(Id = GameObjectId.BottomBorder)] GameObject bottomBorder;
    [Inject(Id = ViewId.PlayerOneScoreLabel)] TMP_Text playerOneLabel;
    [Inject(Id = ViewId.PlayerTwoScoreLabel)] TMP_Text playerTwoLabel;
    [Inject(Id = ViewId.AutoplayToggle)] Toggle autoplayToggle;
    [Inject(Id = ViewId.GameResultLabel)] TMP_Text gameResultLabel;

    Log log;
    int playerOnePoints;
    int playerTwoPoints;
    
    public bool gamePaused { get; private set; }
    
    void Awake() {
        log = new Log(GetType());
        Application.targetFrameRate = settings.targetFrameRate;
    }

    void Start() {
        ballController.setLimits(
            camera.getBottomLeft().x,
            camera.getTopRight().x,
            topBorder.transform.localPosition.y,
            bottomBorder.transform.localPosition.y
        );
        initAutoplayToggle();
        
        assignStickColors();
        startGame();
    }

    void initAutoplayToggle() {
        autoplayToggle.isOn = settings.debug.autoPlay;
        autoplayToggle.onValueChanged.AddListener(value => {
            settings.debug.autoPlay = value;
        });
    }

    void assignStickColors() {
        if (RandomUtils.nextBool()) {
            botStick.spriteRenderer.color = settings.redStickColor;
            playerStick.spriteRenderer.color = settings.blueStickColor;
        } else {
            botStick.spriteRenderer.color = settings.blueStickColor;
            playerStick.spriteRenderer.color = settings.redStickColor;
        }
    }

    void startGame() {
        startServe();
    }

    void startServe() {
        ballController.resetBall();
        botStick.reset();
        playerStick.reset();
        gamePaused = inputController.paused = false;
    }

    void Update() {
        Time.timeScale = settings.timeScale;
    }

    public void onPlayerOneScored() {
        if (gamePaused) return;
        playerOnePoints++;
        playerOneLabel.text = playerOnePoints.ToString();
        onPlayerScored();
    }

    public void onPlayerTwoScored() {
        if (gamePaused) return;
        playerTwoPoints++;
        playerTwoLabel.text = playerTwoPoints.ToString();
        onPlayerScored();
    }

    void onPlayerScored() {
        gamePaused = inputController.paused = true;
        if (playerOnePoints == settings.winPoints) {
            onGameEnd(true);
        } else if (playerTwoPoints == settings.winPoints) {
            onGameEnd(false);
        } else {
            StartCoroutine(Coroutines.delayAction(settings.delayBeforeReset, startServe));
        }
    }

    void onGameEnd(bool playerWon) {
        showResultLabel(playerWon ? "YOU WON!" : "YOU LOST", startGame);
    }

    void showResultLabel(string text, Action action) {
        gameResultLabel.gameObject.SetActive(true);
        gameResultLabel.text = text;
        StartCoroutine(Coroutines.delayAction(settings.resultLabelDuration, () => {
            gameResultLabel.gameObject.SetActive(false);
            resetPoints();
            action.Invoke();
        }));
    }

    void resetPoints() {
        playerOnePoints = playerTwoPoints = 0;
        playerOneLabel.text = playerOnePoints.ToString();
        playerTwoLabel.text = playerTwoPoints.ToString();
    }
}
}