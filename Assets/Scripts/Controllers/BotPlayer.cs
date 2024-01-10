using System.Collections.Generic;
using GameInstaller;
using GameScene;
using Models;
using UnityEngine;
using Utils;
using Utils.Extensions;
using Zenject;

namespace Controllers {
public class BotPlayer : MonoBehaviour {
    [SerializeField] float step = 0.1f;
    [SerializeField] float projectionChangeThreshold = 0.1f;
    [SerializeField] int numberOfProjectionPoints = 30;
    [SerializeField] GameObject projectionPointPrefab;

    [Inject] Ball ball;
    [Inject(Id = StickControllerId.Stick2)] StickController botStick;
    [Inject(Id = StickControllerId.Stick1)] StickController playerStick;
    [Inject] GameController gameController;
    [Inject] GameSettings gameSettings;
    [Inject] new Camera camera;

    Log log;
    float minX;
    float maxX;
    bool isPlayerServing;
    float initialAccuracy;
    float accuracy;
    float maxBallY;
    [SerializeField] Vector3 projectedPosition;

    int numberOfChecks;
    List<GameObject> projectionPoints;
    int pointsIndex;
    GameObject ballProjection;

    void Awake() {
        log = new Log(GetType());
        createProjectionPoints();
        createBallProjection();
    }

    void createProjectionPoints() {
        projectionPoints = new List<GameObject>(numberOfProjectionPoints);
        for (var i = 0; i < numberOfProjectionPoints; i++) {
            var point = Instantiate(projectionPointPrefab);
            point.SetActive(false);
            projectionPoints.Add(point);
        }
    }

    void createBallProjection() {
        ballProjection = Instantiate(projectionPointPrefab);
        ballProjection.GetComponent<SpriteRenderer>().color = Color.yellow;
        ballProjection.GetComponent<SpriteRenderer>().sortingOrder = 3;
        ballProjection.SetActive(gameSettings.debug.showBotPredictedPath);
    }

    void Start() {
        var ballRadius = ball.transform.localScale.x / 2;
        minX = camera.getBottomLeft().x + ballRadius;
        maxX = camera.getTopRight().x - ballRadius;
        maxBallY = botStick.position.y - botStick.transform.localScale.y / 2 - ballRadius;
    }

    void setPointsVisible(bool visible) {
        if (!gameSettings.debug.showBotPredictedPath) return;
        foreach (var point in projectionPoints) {
            point.SetActive(visible);
        }
        ballProjection.gameObject.SetActive(visible);
    }

    void Update() {
        if (gameController.gamePaused) return;
        if (ball.velocity.y > 0) { // the ball is coming
            if (!isPlayerServing) { // initial preparation for receiving the ball
                isPlayerServing = true;
                var accuracyRange = gameSettings.getDifficultySettings().botAccuracy;
                initialAccuracy = RandomUtils.nextFloat(accuracyRange.min, accuracyRange.max);
                accuracy = initialAccuracy;
                setPointsVisible(true);
            }
            var ballPosition = ball.position;
            var value = (ballPosition.y + maxBallY) / (2 * maxBallY);
            // update the accuracy
            accuracy = Mathf.Lerp(initialAccuracy, 1f, Mathf.Pow(value, 2));
            numberOfChecks = 0;
            pointsIndex = 0;
            // update predicted ball position
            var newProjectedPosition = getBallProjectedPosition(ballPosition, ball.velocity);
            if (Mathf.Abs(newProjectedPosition.x - projectedPosition.x) > projectionChangeThreshold) {
                projectedPosition = newProjectedPosition;
            }
            // reduce accuracy due to trajectory complexity
            accuracy -= (numberOfChecks - 1) * gameSettings.getDifficultySettings().trajectoryAccuracyDecrease;
            // apply inaccuracy
            var newPosition = projectedPosition;
            var inaccuracy = botStick.transform.localScale.x * (1 - accuracy);
            var inaccuracyDirection = MathUtils.signOf(botStick.position.x - projectedPosition.x);
            newPosition.x += inaccuracy * inaccuracyDirection;
            // move the stick
            moveStick(newPosition);
            // misc
            ballProjection.transform.localPosition = newProjectedPosition;
            if (pointsIndex > 0) hideUnusedPoints();
        } else { // the ball is moving away
            if (isPlayerServing) {
                isPlayerServing = false;
                setPointsVisible(false);
            }
            moveStick(ball.position);
        }
        if (gameSettings.debug.autoPlay) {
            playerStick.moveStick(ball.position.x - playerStick.position.x, true);
        }
    }

    void hideUnusedPoints() {
        for (var i = pointsIndex; i < projectionPoints.Count; i++) {
            projectionPoints[i].SetActive(false);
        }
    }

    Vector3 getBallProjectedPosition(Vector2 ballPosition, Vector2 ballVelocity) {
        numberOfChecks++;
        var stepVector = ballVelocity.normalized * step;
        var showPoints = gameSettings.debug.showBotPredictedPath;
        var maxChecks = gameSettings.getDifficultySettings().maxTrajectoryLines;
        while (ballPosition.y < maxBallY) {
            ballPosition += stepVector;
            if (showPoints && pointsIndex < projectionPoints.Count) {
                var point = projectionPoints[pointsIndex++];
                point.transform.localPosition = ballPosition;
            }
            if ((ballPosition.x < minX || ballPosition.x > maxX) && numberOfChecks < maxChecks) {
                ballVelocity.x = -ballVelocity.x;
                return getBallProjectedPosition(ballPosition, ballVelocity);
            }
        }
        return new Vector3(ballPosition.x, maxBallY);
    }

    void moveStick(Vector3 targetPosition) {
        var deltaPositionX = targetPosition.x - botStick.position.x;
        var amountX = gameSettings.getDifficultySettings().botMaxSpeed * Time.deltaTime * Mathf.Sign(deltaPositionX);
        if (Mathf.Abs(amountX) > Mathf.Abs(deltaPositionX)) {
            amountX = deltaPositionX;
        }
        botStick.moveStick(amountX);
    }
}
}