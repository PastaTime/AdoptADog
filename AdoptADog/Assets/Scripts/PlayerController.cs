using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    public float speed = 10;
    public float acceleration = 0.8f;
    public Sprite sprite;

    public string horizontalAxis = "Horizontal";
    public string verticalAxis = "Vertical";
    public KeyCode rollKeyCode = KeyCode.F;
    public KeyCode leapKeyCode = KeyCode.G;

    private bool _rolling;
    public bool Rolling
    {
        get { return _rolling; }

        private set
        {
            _spriteRenderer.flipY = value;
            PhysicsCollider.enabled = !value;
            _rolling = value;
        }
    }

    public bool Leaping { get; private set; }

    public Rigidbody2D Rigidbody { get; private set; }
    public Collider2D PhysicsCollider { get; private set; }
    private SpriteRenderer _spriteRenderer;

    private readonly List<GameObject> _handledCollisions = new List<GameObject>();

    private static readonly float rollTime = 0.6f;
    private static readonly float rollMultiplier = 1.2f;
    private static readonly float leapTime = 0.3f;
    private static readonly float leapMultiplier = 1.5f;

    private float _rollStartTime = -1;

    public void SetCollisionHandled(PlayerController other)
    {
        _handledCollisions.Add(other.gameObject);
    }

    private void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        PhysicsCollider = GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = sprite;
    }

    private bool IsDoingAction()
    {
        return Rolling || Leaping;
    }

    private void Update()
    {
        _handledCollisions.Clear();

        float moveHorizontal = Input.GetAxis(horizontalAxis);
        float moveVertical = Input.GetAxis(verticalAxis);

        var movementDir = new Vector2(moveHorizontal, moveVertical).normalized;

        if (Input.GetKeyDown(rollKeyCode))
        {
            Roll();
        }
        if (Input.GetKeyDown(leapKeyCode))
        {
            Leap();
        }

        if (!IsDoingAction())
        {
            Vector2 idealSpeed = movementDir * speed;
            Rigidbody.velocity = Vector2.Lerp(idealSpeed, Rigidbody.velocity, acceleration);
        }
    }

    private void Roll()
    {
        if (IsDoingAction()) return;
        Rolling = true;

        Rigidbody.velocity = Rigidbody.velocity.normalized * speed * rollMultiplier;

        StartCoroutine(RollRoutine());
    }

    private void Leap()
    {
        if (IsDoingAction()) return;
        Leaping = true;

        Rigidbody.velocity = Rigidbody.velocity.normalized * speed * leapMultiplier;

        StartCoroutine(LeapRoutine());
    }

    private IEnumerator RollRoutine()
    {
        _rollStartTime = Time.time;
        yield return new WaitForSeconds(rollTime);
        Rolling = false;
    }

    private IEnumerator LeapRoutine()
    {
        yield return new WaitForSeconds(leapTime);
        Leaping = false;
    }

    private void HandleTriggerCollision(PlayerController other)
    {
        if (other.Rolling) return;
        if (!Rolling) return;

        float v1 = Vector2.Dot(other.Rigidbody.velocity, Rigidbody.velocity);
        float v2 = Rigidbody.velocity.magnitude;
        float relativeVelocity = v2 - v1;
        float timeLeft = rollTime - (Time.time - _rollStartTime);

        // If there's not enough time to pass the player, stop rolling
        if (Math.Abs(relativeVelocity) * timeLeft < other.PhysicsCollider.bounds.size.x + PhysicsCollider.bounds.size.x)
        {
            Rolling = false;
            StopCoroutine(RollRoutine());
        }
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        var otherPlayer = other.gameObject.GetComponent<PlayerController>();
        if (otherPlayer)
        {
            HandleTriggerCollision(otherPlayer);
        }
    }
}
