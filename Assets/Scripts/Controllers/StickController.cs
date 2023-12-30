using System;
using GameInstaller;
using UnityEngine;
using Utils.Extensions;
using Zenject;

namespace Controllers {
public class StickController : MonoBehaviour {
    [Inject] new Camera camera;
    [Inject] GameSettings settings;

    [HideInInspector] public new Transform transform;
    [HideInInspector] public new Rigidbody2D rigidbody;

    public Vector2 position => rigidbody.position;
    
    float minX;
    float maxX;
    float prevPositionX;

    void Awake() {
        transform = GetComponent<Transform>();
        rigidbody = GetComponent<Rigidbody2D>();
        
        var stickHalfWidth = transform.localScale.x / 2;
        minX = camera.getBottomLeft().x + stickHalfWidth;
        maxX = camera.getTopRight().x - stickHalfWidth;
    }

    public void moveStick(float amountX, bool ignoreSpeedLimit = false) {
        var maxAmountX = settings.stickMaxSpeed * Time.deltaTime;
        if (!ignoreSpeedLimit && Mathf.Abs(amountX) > maxAmountX) {
            amountX = maxAmountX * Mathf.Sign(amountX);
        }
        prevPositionX = position.x;
        var newX = Mathf.Clamp(prevPositionX + amountX, minX, maxX);
        rigidbody.MovePosition(new Vector2(newX, position.y));
    }

    public float getDeltaX() => transform.localPosition.x - prevPositionX;
}
}