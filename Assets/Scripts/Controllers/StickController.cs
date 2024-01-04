using GameInstaller;
using UnityEngine;
using Utils;
using Utils.Extensions;
using Zenject;

namespace Controllers {
public class StickController : MonoBehaviour {
    [Inject] new Camera camera;
    [Inject] GameSettings settings;

    [HideInInspector] public new Transform transform;
    [HideInInspector] public new Rigidbody2D rigidbody;
    [HideInInspector] public SpriteRenderer spriteRenderer;

    public Vector2 position {
        get => rigidbody.position;
        private set => rigidbody.position = value;
    }

    Log log;
    float minX;
    float maxX;
    float prevPositionX;

    void Awake() {
        log = new Log(GetType());
        transform = GetComponent<Transform>();
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        var stickHalfWidth = transform.localScale.x / 2;
        minX = camera.getBottomLeft().x + stickHalfWidth;
        maxX = camera.getTopRight().x - stickHalfWidth;
    }

    public void moveStick(float amountX, bool ignoreSpeedLimit = false) {
        if (!ignoreSpeedLimit) {
            var maxAmountX = settings.stickMaxSpeed * Time.deltaTime;
            if (Mathf.Abs(amountX) > maxAmountX) {
                amountX = maxAmountX * Mathf.Sign(amountX);
            }
        }
        prevPositionX = position.x;
        var newX = Mathf.Clamp(prevPositionX + amountX, minX, maxX);
        rigidbody.MovePosition(new Vector2(newX, position.y));
    }

    public void placeStickAt(float newX) {
        var pos = position;
        prevPositionX = pos.x;
        newX = Mathf.Clamp(newX, minX, maxX);
        // rigidbody.MovePosition(new Vector2(newX, position.y));
        position = new Vector2(newX, pos.y);
    }

    public float getDeltaX() => transform.localPosition.x - prevPositionX;

    public void reset() {
        rigidbody.position = new Vector2(0, rigidbody.position.y);
    }
}
}