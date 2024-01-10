using GameInstaller;
using Models;
using services.sounds;
using Services.Vibration;
using UnityEngine;
using Utils;
using Zenject;

namespace Controllers {
public class BallController : MonoBehaviour {
    [SerializeField] float speedIncreaseFactor = 1.01f;
    [SerializeField] float speedTransferFactor = 1f;
    [SerializeField] float maxTransferSpeed = 1f;
    
    [Inject] Ball ball;
    [Inject] GameController gameController;
    [Inject] GameSettings gameSettings;
    [Inject] SoundService soundService;
    [Inject] VibrationService vibrationService;

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
        ball.spriteRenderer.color = gameSettings.ballColor;
        ballHalfRadius = ball.transform.localScale.x / 2;
        playerOneServes = RandomUtils.nextBool();
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
            var deltaX = stickController.getDeltaX() * speedTransferFactor;
            deltaX = Mathf.Min(Mathf.Abs(deltaX), maxTransferSpeed) * Mathf.Sign(deltaX);
            velocity.x += deltaX;
        }
        velocity.y = velocity.y < 0 ? Mathf.Abs(velocity.y) : -Mathf.Abs(velocity.y);
        ball.velocity = velocity.normalized * (velocity.magnitude * speedIncreaseFactor);

        var playerServing = velocity.y > 0;
        soundService.playSound(playerServing ? SoundId.BallHitsPaddle1 : SoundId.BallHitsPaddle2);
        if (playerServing) vibrationService.vibrate(VibrationType.Ms20);
    }

    void Update() {
        var ballPosition = ball.position;
        // check side border collision
        if (ballPosition.x - ballHalfRadius <= leftBorderX) {
            var vel = ball.velocity;
            ball.velocity = new Vector2(Mathf.Abs(vel.x), vel.y);
            if (vel.x < 0) soundService.playSound(SoundId.BallHitsWall1);
        } else if (ballPosition.x + ballHalfRadius >= rightBorderX) {
            var vel = ball.velocity;
            ball.velocity = new Vector2(-Mathf.Abs(vel.x), vel.y);
            if (vel.x > 0) soundService.playSound(SoundId.BallHitsWall1);
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
        else {
            ball.rigidbody.MovePosition(ballPosition + ball.velocity * Time.deltaTime);
        }
    }

    public void resetBall() {
        ball.position = Vector3.zero;
        ballSpeed = gameSettings.getDifficultySettings().ballInitialSpeed;
        var velocity = new Vector3(RandomUtils.nextFloatWithRandomSign(0f, 0.5f), playerOneServes ? 1f : -1f);
        ball.velocity = velocity.normalized * (ballSpeed / 2);
        ballSpeedReduced = true;
    }
}
}