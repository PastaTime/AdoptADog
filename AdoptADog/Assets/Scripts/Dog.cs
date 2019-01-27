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
    public AudioManager manager;
    
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
            if (_posing == value) return;
            _posing = value;
            _animator.SetBool(PosingAnimationId, value);
        }
    }

    private bool _stunned;
    public bool Stunned
    {
        get => _stunned;
        set
        {
            _stunned = value;
            _animator.SetBool(StunnedAnimationId, value);
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
        ActionTime = 0.4f,
        Cooldown = 0.7f,
        SpeedMultiplier = 1.2f,
    };

    private readonly DogAction _leap = new DogAction()
    {
        ActionTime = 0.3f,
        Cooldown = 0.7f,
        SpeedMultiplier = 1.5f,
    };

    private readonly DogAction _pose = new DogAction()
    {
        ActionTime = 1f,
        Cooldown = 0.5f,
        SpeedMultiplier = 0f,
    };

    private readonly DogAction _stun = new DogAction()
    {
        ActionTime = 0.9f,
        Cooldown = 0.0f,
        SpeedMultiplier = 0f,
    };

    private static readonly int RollingAnimationId = Animator.StringToHash("Rolling");
    private static readonly int LeapingAnimationId = Animator.StringToHash("Leaping");
    private static readonly int PosingAnimationId = Animator.StringToHash("Posing");
    private static readonly int VelocityAnimationId = Animator.StringToHash("Velocity");
    private static readonly int StunnedAnimationId = Animator.StringToHash("Stunned");

    private bool _doPose = false;

    public Vector2 MovementDir { get; set; }

    private void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        PhysicsCollider = GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _spotlight = FindObjectOfType<SpotlightScript>();
        manager = FindObjectOfType<AudioManager>();
    }

    private void Update()
    {
        if (Stunned)
        {
            Rigidbody.velocity = Vector2.Lerp(Vector2.zero, Rigidbody.velocity, acceleration);
        }

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
        Posing = _doPose;
        _doPose = false;
    }

    public void SetCollisionHandled(Dog other)
    {
        manager.PlayAudio(manager.playerContact);
        _handledCollisions.Add(other.gameObject);
    }

    public void Stun(float stunTime = -1)
    {
        if (!CanDoAction(_stun)) return;

        Stunned = true;
        if (stunTime < 0) stunTime = _stun.ActionTime;
        StartCoroutine(StunRoutine(stunTime));

    }

    public void Roll()
    {
        if (!CanDoAction(_roll)) return;
        Rolling = true;

        Rigidbody.velocity = MovementDir * Speed * _roll.SpeedMultiplier;
        
        manager.PlayAudio(manager.playerRoll);

        StartCoroutine(RollRoutine());
    }

    public void Leap()
    {
        if (!CanDoAction(_leap)) return;
        Leaping = true;

        Rigidbody.velocity = MovementDir * Speed * _leap.SpeedMultiplier;
        
        manager.PlayAudio(manager.playerLeap);

        StartCoroutine(LeapRoutine());
    }

    public void Pose(bool withSound = false, bool force = false)
    {
        if ((Rolling || Leaping || Stunned) && !force) return;
        if (!Posing && !force)
        {
            if (Time.time < _pose.TimeFinished + _pose.Cooldown) return;
            manager.PlayAudio(manager.playerPose);
        }

        _doPose = true;

        Rigidbody.velocity = Vector2.zero;
        _pose.TimeFinished = Time.time;

        if (_spotlight != null && _spotlight.InSpotlight(name))
        {
            PointManager.GetSingleton().AddPosePoints(PlayerNumber, Time.deltaTime);
        }
    }

    public void PushSomeone()
    {
        if (PlayerNumber < 0 || !_spotlight.InSpotlight(name)) return;
        PointManager.GetSingleton().AddPushPoints(PlayerNumber);
    }

    private IEnumerator RollRoutine()
    {
        _roll.TimeStarted = Time.time;
        yield return new WaitForSeconds(_roll.ActionTime);
        _roll.TimeFinished = Time.time;
        Rolling = false;
    }

    private IEnumerator StunRoutine(float stunTime)
    {
        _stun.TimeStarted = Time.time;
        yield return new WaitForSeconds(stunTime);
        _stun.TimeFinished = Time.time;
        Stunned = false;
    }

    private IEnumerator LeapRoutine()
    {
        yield return new WaitForSeconds(_leap.ActionTime);
        Leaping = false;
        _leap.TimeFinished = Time.time;
    }

    private IEnumerator PoseRoutine()
    {
        yield return new WaitForSeconds(_pose.ActionTime);
        Posing = false;
        _pose.TimeFinished = Time.time;
    }

    private bool CanDoAction(DogAction action = null)
    {
        if (action == _stun) return true;
        if (Rolling || Leaping || (action != _pose && Posing) || Stunned) return false;
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
        var wall = other.gameObject;
        if (otherPlayer)
        {
            // Don't handle collisions twice
            if (_handledCollisions.Contains(other.gameObject)) return;
            CollisionUtils.HandlePlayerCollision(this, otherPlayer);
        }

        if (wall.layer == LayerMask.NameToLayer("Wall"))
        {
            Vector2 pos = new Vector2(wall.transform.position.x, wall.transform.position.y);
            Vector2 dir = (pos - Rigidbody.position).normalized;
            float speedToWall = Vector2.Dot(Rigidbody.velocity, dir);
            Debug.Log(speedToWall);
//            Stun(0.5f);
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