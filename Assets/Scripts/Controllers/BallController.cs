using System;
using Models;
using UnityEngine;
using Utils;
using Utils.Extensions;
using Zenject;

namespace Controllers {
public class BallController : MonoBehaviour {
    [Inject] Ball ball;
    [Inject] GameController gameController;

    Log log;
    float leftBorderX;
    float rightBorderX;
    float topLineY;
    float bottomLineY;
    float ballHalfRadius;

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
        ball.velocity = velocity.y < 0
            ? new Vector3(velocity.x, Mathf.Abs(velocity.y))
            : new Vector3(velocity.x, -Mathf.Abs(velocity.y));
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
        ball.velocity = new Vector3(RandomUtils.nextFloatWithRandomSign(0.5f, 1f), RandomUtils.nextSign(3f));
    }
}
}