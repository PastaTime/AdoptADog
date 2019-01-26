using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    public float speed = 10;
    public float acceleration = 0.8f;

    public int playerNumber = 1;

    private bool _rolling;
    public bool Rolling
    {
        get => _rolling;

        private set
        {
            _spriteRenderer.flipY = value;
            PhysicsCollider.enabled = !value;
            _rolling = value;
            _animator.SetBool(RollingAnimationId, value);
        }
    }

    public bool Leaping { get; private set; }

    public Rigidbody2D Rigidbody { get; private set; }
    public Collider2D PhysicsCollider { get; private set; }
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    private readonly List<GameObject> _handledCollisions = new List<GameObject>();

    private readonly PlayerAction _roll = new PlayerAction()
    {
        ActionTime = 0.6f,
        Cooldown = 1.5f,
        SpeedMultiplier = 1.2f,
    };

    private readonly PlayerAction _leap = new PlayerAction()
    {
        ActionTime = 0.3f,
        Cooldown = 1f,
        SpeedMultiplier = 1.5f,
    };

    private static readonly int RollingAnimationId = Animator.StringToHash("Rolling");
    private static readonly int VelocityAnimationId = Animator.StringToHash("Velocity");

    public void SetCollisionHandled(PlayerController other)
    {
        _handledCollisions.Add(other.gameObject);
    }

    private void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        PhysicsCollider = GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    private bool CanDoAction(PlayerAction action = null)
    {
        if (Rolling || Leaping) return false;

        if (action == null) return true;

        Debug.Log("Finished " + action.TimeFinished);
        Debug.Log("Time " + Time.time);
        return Time.time > action.TimeFinished + action.Cooldown;
    }

    private Vector2 GetMovementDir() {
        float moveHorizontal = Controller.getSingleton().getHorizontal(playerNumber);
        float moveVertical = Controller.getSingleton().getVertical(playerNumber);

        return new Vector2(moveHorizontal, moveVertical).normalized;
    }

    private void Update()
    {
        _handledCollisions.Clear();

        Vector2 movementDir = GetMovementDir();

        if (Controller.getSingleton().getA(playerNumber))
        {
            Roll();
        }
        if (Controller.getSingleton().getB(playerNumber))
        {
            Leap();
        }

        if (CanDoAction())
        {
            Vector2 idealSpeed = movementDir * speed;
            Rigidbody.velocity = Vector2.Lerp(idealSpeed, Rigidbody.velocity, acceleration);
        }

        if (Rigidbody.velocity.x == 0)
        {
            //  nothing
        }
        else if (Rigidbody.velocity.x > 0)
        {
            _spriteRenderer.flipX = true;
        }
        else
        {
            _spriteRenderer.flipX = false;
        }

        _animator.SetFloat(VelocityAnimationId, Rigidbody.velocity.magnitude);
    }

    private void Roll()
    {
        if (!CanDoAction(_roll)) return;
        Rolling = true;

        Rigidbody.velocity = GetMovementDir() * speed * _roll.SpeedMultiplier;

        StartCoroutine(RollRoutine());
    }

    private void Leap()
    {
        if (!CanDoAction(_leap)) return;
        Leaping = true;

        Rigidbody.velocity = Rigidbody.velocity.normalized * speed * _leap.SpeedMultiplier;

        StartCoroutine(LeapRoutine());
    }

    private IEnumerator RollRoutine()
    {
        _roll.TimeStarted = Time.time;
        yield return new WaitForSeconds(_roll.ActionTime);
        _roll.TimeFinished = Time.time;
        Rolling = false;
    }

    private IEnumerator LeapRoutine()
    {
        yield return new WaitForSeconds(_leap.ActionTime);
        Leaping = false;
        _leap.TimeFinished = Time.time;
    }

    private void HandleTriggerCollision(PlayerController other)
    {
        if (other.Rolling) return;
        if (!Rolling) return;

        float v1 = Vector2.Dot(other.Rigidbody.velocity, Rigidbody.velocity);
        float v2 = Rigidbody.velocity.magnitude;
        float relativeVelocity = v2 - v1;
        float timeLeft = _roll.ActionTime - (Time.time - _roll.TimeStarted);

        // If there's not enough time to pass the player, stop rolling
        if (Math.Abs(relativeVelocity) * timeLeft < other.PhysicsCollider.bounds.size.x + PhysicsCollider.bounds.size.x)
        {
            Rolling = false;
            _roll.TimeFinished = Time.time;
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


public class PlayerAction
{
    public float ActionTime { get; set; }
    public float SpeedMultiplier { get; set; }
    public float Cooldown { get; set; }
    public float TimeStarted { get; set; } = float.MinValue;
    public float TimeFinished { get; set; } = float.MinValue;
}