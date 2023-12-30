using System;
using GameInstaller;
using GameScene;
using Models;
using UnityEngine;
using Zenject;

namespace Controllers {
public class BotPlayer : MonoBehaviour {
    [Inject] Ball ball;
    [Inject(Id = StickControllerId.Stick2)] StickController stickController;
    [Inject] GameSettings settings;

    void Update() {
        var deltaPositionX = ball.position.x - stickController.stickPosition.x;
        var deltaX = settings.stickMaxSpeed * Time.deltaTime * Mathf.Sign(deltaPositionX);
        if (Mathf.Abs(deltaPositionX) < Mathf.Abs(deltaX)) {
            deltaX = deltaPositionX;
        }
        stickController.moveStick(deltaX);
    }
}
}