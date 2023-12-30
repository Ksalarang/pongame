﻿using System;
using GameInstaller;
using Models;
using services.sounds;
using UnityEngine;
using Utils;
using Utils.Extensions;
using Zenject;

namespace Controllers {
public class BallController : MonoBehaviour {
    [SerializeField] float speedIncreaseFactor = 1.01f;
    [SerializeField] float speedTransferFactor = 1f;
    
    [Inject] Ball ball;
    [Inject] GameController gameController;
    [Inject] GameSettings gameSettings;
    [Inject] SoundService soundService;

    Log log;
    float leftBorderX;
    float rightBorderX;
    float topLineY;
    float bottomLineY;
    float ballHalfRadius;

    float ballSpeed;
    bool ballSpeedReduced;
    bool playerOneServes;

    void Awake() {
        log = new Log(GetType());
    }

    void Start() {
        ball.onCollisionEnter = onBallCollisionEnter;
        ballHalfRadius = ball.transform.localScale.x / 2;
        playerOneServes = RandomUtils.nextBool();
        resetBall();
    }

    public void setLimits(float leftBorderX, float rightBorderX, float topLineY, float bottomLineY) {
        this.leftBorderX = leftBorderX;
        this.rightBorderX = rightBorderX;
        this.topLineY = topLineY;
        this.bottomLineY = bottomLineY;
    }

    void onBallCollisionEnter(Collider2D other) {
        var velocity = ball.velocity;
        if (ballSpeedReduced) {
            velocity *= 2;
            ballSpeedReduced = false;
        }
        var stickController = other.GetComponent<StickController>();
        if (stickController) {
            velocity.x += stickController.getDeltaX() * speedTransferFactor;
        }
        velocity.y = velocity.y < 0 ? Mathf.Abs(velocity.y) : -Mathf.Abs(velocity.y);
        ball.velocity = velocity.normalized * (velocity.magnitude * speedIncreaseFactor);
        
        soundService.playSound(SoundId.BallHit1);
    }

    void Update() {
        var ballPosition = ball.position;
        // check side border collision
        if (ballPosition.x - ballHalfRadius <= leftBorderX) {
            var vel = ball.velocity;
            ball.velocity = new Vector2(Mathf.Abs(vel.x), vel.y);
        } else if (ballPosition.x + ballHalfRadius >= rightBorderX) {
            var vel = ball.velocity;
            ball.velocity = new Vector2(-Mathf.Abs(vel.x), vel.y);
        }
        
        if (gameController.gamePaused) return;
        
        // check if ball is outside the field
        if (ballPosition.y > topLineY) {
            gameController.onPlayerOneScored();
            playerOneServes = true;
        } else if (ballPosition.y < bottomLineY) {
            gameController.onPlayerTwoScored();
            playerOneServes = false;
        }
        // update position
        ball.rigidbody.MovePosition(ballPosition + ball.velocity * Time.deltaTime);
    }

    public void resetBall() {
        Log.log("BallController", "reset ball");
        ball.position = Vector3.zero;
        ballSpeed = gameSettings.getDifficultySettings().ballInitialSpeed;
        var velocity = new Vector3(RandomUtils.nextFloatWithRandomSign(0f, 0.5f), playerOneServes ? 1f : -1f);
        ball.velocity = velocity.normalized * ballSpeed / 2;
        ballSpeedReduced = true;
    }
}
}