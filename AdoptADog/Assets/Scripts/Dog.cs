using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Dog : MonoBehaviour
{
    public float Speed { get; set; } = 7;
    private const float acceleration = 0.9f;
    public int PlayerNumber { get; set; } = -1;

    private bool _rolling;
    public bool Rolling
    {
        get => _rolling;

        private set
        {
            _spriteRenderer.flipY = value;
            gameObject.layer = value ? LayerMask.NameToLayer("Dodge") : LayerMask.NameToLayer("Dog");
            _rolling = value;
            _animator.SetBool(RollingAnimationId, value);
        }
    }

    private bool _leaping;
    public bool Leaping
    {
        get => _leaping;
        private set
        {
            _leaping = value;
            _animator.SetBool(LeapingAnimationId, value);
        }
    }

    private bool _posing;
    public bool Posing
    {
        get => _posing;
        set
        {
            _posing = value;
            _animator.SetBool(PosingAnimationId, value);
        }
    }

    public Rigidbody2D Rigidbody { get; private set; }
    public Collider2D PhysicsCollider { get; private set; }
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private SpotlightScript _spotlight;

    private readonly List<GameObject> _handledCollisions = new List<GameObject>();

    private readonly DogAction _roll = new DogAction()
    {
        ActionTime = 0.6f,
        Cooldown = 1f,
        SpeedMultiplier = 1.2f,
    };

    private readonly DogAction _leap = new DogAction()
    {
        ActionTime = 0.3f,
        Cooldown = 0.7f,
        SpeedMultiplier = 1.5f,
    };

    private static readonly int RollingAnimationId = Animator.StringToHash("Rolling");
    private static readonly int LeapingAnimationId = Animator.StringToHash("Leaping");
    private static readonly int PosingAnimationId = Animator.StringToHash("Posing");
    private static readonly int VelocityAnimationId = Animator.StringToHash("Velocity");

    public Vector2 MovementDir { get; set; }

    private void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        PhysicsCollider = GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _spotlight = FindObjectOfType<SpotlightScript>();
    }

    private void Update()
    {
        // Only move freely when you can do an action
        if (CanDoAction())
        {
            Vector2 idealSpeed = MovementDir * Speed;
            Rigidbody.velocity = Vector2.Lerp(idealSpeed, Rigidbody.velocity, acceleration);
        }

        if (Rigidbody.velocity.x > 0)
        {
            _spriteRenderer.flipX = true;
        }
        else if (Rigidbody.velocity.x < 0)
        {
            _spriteRenderer.flipX = false;
        }

        _animator.SetFloat(VelocityAnimationId, Rigidbody.velocity.magnitude);
    }

    public void SetCollisionHandled(Dog other)
    {
        _handledCollisions.Add(other.gameObject);
    }

    public void Roll()
    {
        if (!CanDoAction(_roll)) return;
        Rolling = true;

        Rigidbody.velocity = MovementDir * Speed * _roll.SpeedMultiplier;

        StartCoroutine(RollRoutine());
    }

    public void Leap()
    {
        if (!CanDoAction(_leap)) return;
        Leaping = true;

        Rigidbody.velocity = Rigidbody.velocity.normalized * Speed * _leap.SpeedMultiplier;

        StartCoroutine(LeapRoutine());
    }

    public void PushSomeone()
    {
        if (PlayerNumber < 0) return;
        PointManager.GetSingleton().AddPosePoints(PlayerNumber);
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

    private bool CanDoAction(DogAction action = null)
    {
        if (Rolling || Leaping) return false;
        if (action == null) return true;
        return Time.time > action.TimeFinished + action.Cooldown;
    }

    private void HandleTriggerCollision(Dog other)
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
        var otherPlayer = other.gameObject.GetComponent<Dog>();
        if (otherPlayer)
        {
            // Don't handle collisions twice
            if (_handledCollisions.Contains(other.gameObject)) return;
            CollisionUtils.HandlePlayerCollision(this, otherPlayer);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var otherPlayer = other.gameObject.GetComponent<Dog>();
        if (otherPlayer)
        {
            HandleTriggerCollision(otherPlayer);
        }
    }

}

public class DogAction
{
    public float ActionTime { get; set; }
    public float SpeedMultiplier { get; set; }
    public float Cooldown { get; set; }
    public float TimeStarted { get; set; } = float.MinValue;
    public float TimeFinished { get; set; } = float.MinValue;
}