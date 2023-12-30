﻿using System;
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
    [SerializeField] float movementThreshold = 0.1f;
    [SerializeField] int numberOfProjectionPoints = 30;
    [SerializeField] GameObject projectionPointPrefab;
    
    [Inject] Ball ball;
    [Inject(Id = StickControllerId.Stick2)] StickController botStick;
    [Inject(Id = StickControllerId.Stick1)] StickController playerStick;
    [Inject] GameSettings gameSettings;
    [Inject] new Camera camera;

    float minX;
    float maxX;
    bool ballStartedNearing;
    float accuracy;

    int numberOfChecks;
    List<GameObject> projectionPoints;
    int pointsIndex;

    void Awake() {
        createProjectionPoints();
    }

    void Start() {
        var ballHalfRadius = ball.transform.localScale.x / 2;
        minX = camera.getBottomLeft().x + ballHalfRadius;
        maxX = camera.getTopRight().x - ballHalfRadius;
    }

    void createProjectionPoints() {
        projectionPoints = new List<GameObject>(numberOfProjectionPoints);
        for (var i = 0; i < numberOfProjectionPoints; i++) {
            var point = Instantiate(projectionPointPrefab);
            point.SetActive(false);
            projectionPoints.Add(point);
        }
    }

    void setPointsVisible(bool visible) {
        if (!gameSettings.debug.showBotPredictedPath) return;
        foreach (var point in projectionPoints) {
            point.SetActive(visible);
        }
    }

    void Update() {
        if (ball.velocity.y > 0) {
            var errorSign = 1f;
            if (!ballStartedNearing) {
                ballStartedNearing = true;
                var accuracyRange = gameSettings.getDifficultySettings().botAccuracy;
                accuracy = RandomUtils.nextFloat(accuracyRange.min, accuracyRange.max);
                errorSign = RandomUtils.nextSign();
                setPointsVisible(true);
            }
            var ballPosition = ball.position;
            var maxBallY = getMaxBallY();
            var value = (ballPosition.y + maxBallY) / (2 * maxBallY);
            accuracy = Mathf.Lerp(accuracy, 1f, Mathf.Pow(value, 2));
            numberOfChecks = 0;
            pointsIndex = 0;
            var projectedPosition = getBallProjectedPosition(ballPosition, ball.velocity);
            if (pointsIndex > 0) hideUnusedPoints();
            var error = botStick.transform.localScale.x * (1 - accuracy);
            projectedPosition.x += error * errorSign;
            moveStick(projectedPosition);
        } else {
            if (ballStartedNearing) {
                ballStartedNearing = false;
                setPointsVisible(false);
            }
            if (gameSettings.debug.autoPlay) {
                playerStick.moveStick(ball.position.x - playerStick.position.x, true);
            }
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
        var maxBallY = getMaxBallY();
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

    float getMaxBallY() {
        return botStick.position.y - botStick.transform.localScale.y / 2 - ball.transform.localScale.y / 2;
    }

    void moveStick(Vector3 targetPosition) {
        var deltaX = targetPosition.x - botStick.position.x;
        if (Mathf.Abs(deltaX) < movementThreshold) {
            deltaX = 0f;
        }
        var amountX = gameSettings.getDifficultySettings().botMaxSpeed * Time.deltaTime * Mathf.Sign(deltaX);
        if (Mathf.Abs(amountX) > Mathf.Abs(deltaX)) {
            amountX = deltaX;
        }
        botStick.moveStick(amountX);
    }
}
}