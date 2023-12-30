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

    public Vector2 stickPosition => rigidbody.position;
    
    float minX;
    float maxX;

    void Awake() {
        transform = GetComponent<Transform>();
        rigidbody = GetComponent<Rigidbody2D>();
        
        var stickHalfWidth = transform.localScale.x / 2;
        minX = camera.getBottomLeft().x + stickHalfWidth;
        maxX = camera.getTopRight().x - stickHalfWidth;
    }

    public void moveStick(float deltaX) {
        var maxDeltaX = settings.stickMaxSpeed * Time.deltaTime;
        if (Mathf.Abs(deltaX) > maxDeltaX) {
            deltaX = maxDeltaX * Mathf.Sign(deltaX);
        }
        var position = transform.localPosition;
        var newX = Mathf.Clamp(position.x + deltaX, minX, maxX);
        rigidbody.MovePosition(new Vector2(newX, position.y));
    }
}
}