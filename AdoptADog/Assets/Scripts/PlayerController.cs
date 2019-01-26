using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    public float speed = 3;
    public float acceleration = 0.8f;
    public Sprite sprite;

    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;

    public string horizontalAxis = "Horizontal";
    public string verticalAxis = "Vertical";

    void Start ()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = sprite;
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis(horizontalAxis);
        float moveVertical = Input.GetAxis(verticalAxis);

        var movementDir = new Vector2(moveHorizontal, moveVertical).normalized;

        Vector2 idealSpeed = movementDir * speed;

        _rigidbody.velocity = Vector2.Lerp(idealSpeed, _rigidbody.velocity, acceleration);

//        _rigidbody.AddForce(force);
    }
}
