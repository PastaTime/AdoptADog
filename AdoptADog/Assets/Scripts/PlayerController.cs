using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    public float speed = 3;
    public float acceleration = 0.8f;
    public Sprite sprite;

    public string horizontalAxis = "Horizontal";
    public string verticalAxis = "Vertical";

    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    private readonly List<GameObject> _handledCollisions = new List<GameObject>();

    private void Start ()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = sprite;
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _handledCollisions.Clear();

        float moveHorizontal = Input.GetAxis(horizontalAxis);
        float moveVertical = Input.GetAxis(verticalAxis);

        var movementDir = new Vector2(moveHorizontal, moveVertical).normalized;

        if (Input.GetKeyDown(KeyCode.H))
        {
            _rigidbody.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
        }

        Vector2 idealSpeed = movementDir * speed;
        _rigidbody.velocity = Vector2.Lerp(idealSpeed, _rigidbody.velocity, acceleration);

        if (_rigidbody.velocity.x == 0)
        {
            //  nothing
        }
        else if (_rigidbody.velocity.x > 0)
        {
            _spriteRenderer.flipX = true;
        }
        else
        {
            _spriteRenderer.flipX = false;
        }

        _animator.SetFloat("Velocity", _rigidbody.velocity.magnitude);

    }

    public void SetCollisionHandled(PlayerController other)
    {
        _handledCollisions.Add(other.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Only handle player collisions
        var otherPlayer = other.gameObject.GetComponent<PlayerController>();
        if (otherPlayer)
        {
            // Don't handle collisions twice
            if (_handledCollisions.Contains(other.gameObject)) return;
            CollisionUtils.HandlePlayerCollision(this, otherPlayer);
        }
    }
}
