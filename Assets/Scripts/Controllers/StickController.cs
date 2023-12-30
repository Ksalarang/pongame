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

    void Awake() {
        transform = GetComponent<Transform>();
        rigidbody = GetComponent<Rigidbody2D>();
        
        var stickHalfWidth = transform.localScale.x / 2;
        minX = camera.getBottomLeft().x + stickHalfWidth;
        maxX = camera.getTopRight().x - stickHalfWidth;
    }

    public void moveStick(float amountX) {
        var maxAmountX = settings.stickMaxSpeed * Time.deltaTime;
        if (Mathf.Abs(amountX) > maxAmountX) {
            amountX = maxAmountX * Mathf.Sign(amountX);
        }
        var newX = Mathf.Clamp(position.x + amountX, minX, maxX);
        rigidbody.MovePosition(new Vector2(newX, position.y));
    }
}
}