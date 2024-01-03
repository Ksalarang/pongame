using System;
using UnityEngine;

namespace Models {
public class Ball : MonoBehaviour {
    [HideInInspector] public new Transform transform;
    [HideInInspector] public new CircleCollider2D collider;
    [HideInInspector] public new Rigidbody2D rigidbody;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    
    [HideInInspector] public Vector2 velocity;
    
    public Action<Collider2D> onCollisionEnter;

    public Vector2 position {
        get => rigidbody.position;
        set => rigidbody.MovePosition(value);
    }

    void Awake() {
        transform = base.transform;
        collider = GetComponent<CircleCollider2D>();
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        onCollisionEnter?.Invoke(other);
    }
}
}