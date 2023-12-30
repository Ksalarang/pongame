using System;
using GameInstaller;
using Models;
using UnityEngine;
using Utils;
using Utils.Extensions;
using Zenject;

namespace Controllers {
public class BallController : MonoBehaviour {
    [SerializeField] float speedIncreaseFactor = 1.01f;
    [Inject] Ball ball;
    [Inject] GameController gameController;
    [Inject] GameSettings gameSettings;

    Log log;
    float leftBorderX;
    float rightBorderX;
    float topLineY;
    float bottomLineY;
    float ballHalfRadius;

    float ballSpeed;

    void Awake() {
        log = new Log(GetType());
    }

    void Start() {
        ball.onCollisionEnter = onBallCollisionEnter;
        ballHalfRadius = ball.transform.localScale.x / 2;
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
        velocity.y = velocity.y < 0 ? Mathf.Abs(velocity.y) : -Mathf.Abs(velocity.y);
        ball.velocity = velocity.normalized * (velocity.magnitude * speedIncreaseFactor);
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
        if (ballPosition.y < bottomLineY) {
            gameController.onPlayerTwoScored();
        } else if (ballPosition.y > topLineY) {
            gameController.onPlayerOneScored();
        }
        // update position
        ball.rigidbody.MovePosition(ballPosition + ball.velocity * Time.deltaTime);
    }

    public void resetBall() {
        Log.log("BallController", "reset ball");
        ball.position = Vector3.zero;
        ballSpeed = gameSettings.getDifficultySettings().ballInitialSpeed;
        var velocity = new Vector3(RandomUtils.nextFloatWithRandomSign(), RandomUtils.nextSign());
        ball.velocity = velocity.normalized * ballSpeed;
    }
}
}