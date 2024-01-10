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
    [Header("Animations")]
    [SerializeField] float scoreLabelScale;
    [SerializeField] float scoreLabelScaleDuration;
    
    [Inject] GameSettings settings;
    [Inject] new Camera camera;
    [Inject] BallController ballController;
    [Inject] InputController inputController;
    [Inject(Id = StickControllerId.Stick1)] StickController playerStick;
    [Inject(Id = StickControllerId.Stick2)] StickController botStick;
    [Inject] Ball ball;

    [Inject(Id = GameObjectId.TopBorder)] GameObject topBorder;
    [Inject(Id = GameObjectId.BottomBorder)] GameObject bottomBorder;
    [Inject(Id = UIElementId.PlayerOneScoreLabel)] TMP_Text playerOneLabel;
    [Inject(Id = UIElementId.PlayerTwoScoreLabel)] TMP_Text playerTwoLabel;
    [Inject(Id = UIElementId.AutoplayToggle)] Toggle autoplayToggle;
    [Inject(Id = UIElementId.GameResultLabel)] TMP_Text gameResultLabel;

    [Inject] SettingsWindow settingsWindow;

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
        resetBallAndSticks();
        gamePaused = inputController.paused = false;
    }

    void resetBallAndSticks() {
        ballController.resetBall();
        botStick.reset();
        playerStick.reset();
    }

    void Update() {
        Time.timeScale = settings.timeScale;
    }

    public void onPlayerOneScored() {
        if (gamePaused) return;
        playerOnePoints++;
        playerOneLabel.text = playerOnePoints.ToString();
        animateScoreLabel(playerOneLabel);
        onPlayerScored();
    }

    public void onPlayerTwoScored() {
        if (gamePaused) return;
        playerTwoPoints++;
        playerTwoLabel.text = playerTwoPoints.ToString();
        animateScoreLabel(playerTwoLabel);
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
        showResultLabel(playerWon ? "YOU WON!" : "YOU LOST", () => {
            resetPoints();
            startGame();
        });
    }

    void showResultLabel(string text, Action action) {
        gameResultLabel.gameObject.SetActive(true);
        gameResultLabel.text = text;
        StartCoroutine(Coroutines.delayAction(settings.resultLabelDuration, () => {
            gameResultLabel.gameObject.SetActive(false);
            action.Invoke();
        }));
    }

    void resetPoints() {
        playerOnePoints = playerTwoPoints = 0;
        playerOneLabel.text = playerOnePoints.ToString();
        playerTwoLabel.text = playerTwoPoints.ToString();
    }

    void animateScoreLabel(TMP_Text scoreLabel) {
        StartCoroutine(Coroutines.scaleToAndBack(scoreLabel.transform,
            scoreLabel.transform.localScale * scoreLabelScale,
            scoreLabelScaleDuration, false));
    }

    public void onClickSettingsButton() {
        gamePaused = true;
        settingsWindow.onHideAction = () => {
            gamePaused = false;
        };
        settingsWindow.show();
    }

    public void resetCurrentGame() {
        resetPoints();
        resetBallAndSticks();
    }
}
}