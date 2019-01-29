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

    public bool Rolling
    {
        get => _roll.Active;
        set => _roll.Active = value;
    }

    public bool Leaping => _leap.Active;
    public bool Stunned => _stun.Active;

    public bool Posing
    {
        get => _pose.Active;
        private set
        {
            if (_pose.Active == value) return;
            _pose.Active = value;
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
        ActionTime = 0.4f,
        Cooldown = 0.7f,
        SpeedMultiplier = 1.2f,
        OnChange = (dog, value) =>
        {
            dog.gameObject.layer = value ? LayerMask.NameToLayer("Dodge") : LayerMask.NameToLayer("Dog");
            dog._animator.SetBool(RollingAnimationId, value);
        }
    };

    private readonly DogAction _leap = new DogAction()
    {
        ActionTime = 0.3f,
        Cooldown = 0.7f,
        SpeedMultiplier = 1.5f,
        OnChange = (dog, value) =>
        {
            dog._animator.SetBool(LeapingAnimationId, value);
        }
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
        OnChange = (dog, value) =>
        {
            dog._animator.SetBool(StunnedAnimationId, value);
        }
    };

    private static readonly int RollingAnimationId = Animator.StringToHash("Rolling");
    private static readonly int LeapingAnimationId = Animator.StringToHash("Leaping");
    private static readonly int PosingAnimationId = Animator.StringToHash("Posing");
    private static readonly int WalkingAnimationId = Animator.StringToHash("Walking");
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

        if (manager == null) return;
        _leap.Audio = manager.playerLeap;
        _pose.Audio = manager.playerPose;
        _roll.Audio = manager.playerRoll;
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

        _animator.SetFloat(WalkingAnimationId, Math.Abs(Rigidbody.velocity.magnitude) > 0.1 ? 1 : 0);
        Posing = _doPose;
        _doPose = false;
    }

    public void SetCollisionHandled(Dog other)
    {
        manager.PlayAudio(manager.playerContact);
        _handledCollisions.Add(other.gameObject);
    }

    public void Roll() => DoAction(_roll);
    public void Leap() => DoAction(_leap);
    public void Stun() => DoAction(_stun);

    private void DoAction(DogAction action)
    {
        if (!CanDoAction(action)) return;
        action.Active = true;
        action.OnChange(this, true);

        if (action != _stun)
        {
            Rigidbody.velocity = MovementDir * Speed * action.SpeedMultiplier;
        }

        if (action.Audio != null) manager.PlayAudio(action.Audio);

        StartCoroutine(ActionRoutine(action));
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

    private IEnumerator ActionRoutine(DogAction action)
    {
        action.TimeStarted = Time.time;
        yield return new WaitForSeconds(action.ActionTime);
        action.TimeFinished = Time.time;
        action.Active = false;
        action.OnChange(this, false);
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
            _roll.OnChange(this, false);
            _roll.TimeFinished = Time.time;
            StopCoroutine(ActionRoutine(_roll));
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

public delegate void ChangeAction(Dog dog, bool value);

public class DogAction
{
    public float ActionTime { get; set; }
    public float SpeedMultiplier { get; set; }
    public float Cooldown { get; set; }
    public float TimeStarted { get; set; } = float.MinValue;
    public float TimeFinished { get; set; } = float.MinValue;
    public bool Active { get; set; }
    public AudioClip Audio { get; set; }
    public ChangeAction OnChange { get; set; }
}