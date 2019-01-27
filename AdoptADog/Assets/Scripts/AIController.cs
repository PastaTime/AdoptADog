
using System;
using System.Collections;
using UnityEngine;
using Random = System.Random;

[RequireComponent(typeof(Dog))]
public class AIController : MonoBehaviour
{
    private Dog _dog;
    private static Random _random = new Random();
    private Vector2 moveDirection = new Vector2();

    private void Start()
    {
        _dog = GetComponent<Dog>();
        _dog.Speed = 3;
        StartCoroutine(Movement());
    }

    private IEnumerator Movement()
    {
        _dog.MovementDir = new Vector2(_random.Next(-100, 100), _random.Next(-100, 100)).normalized;
        yield return new WaitForSeconds(_random.Next(100, 200) / 100f);
        StartCoroutine(Movement());
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        // Only handle player collisions
        if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            StopAllCoroutines();
            StartCoroutine(Movement());
        }
    }

}
