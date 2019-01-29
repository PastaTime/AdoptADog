using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

[RequireComponent(typeof(Dog))]
public class PlayerController : MonoBehaviour
{
    public int playerNumber = 1;
    private Dog _dog;

    public bool CanMove { get; set; } = true;

    private void Start()
    {
        _dog = GetComponent<Dog>();
        _dog.PlayerNumber = playerNumber;
    }

    private Vector2 GetMovementDir()
    {
        float moveHorizontal = Controller.GetSingleton().GetHorizontal(playerNumber);
        float moveVertical = Controller.GetSingleton().GetVertical(playerNumber);

        return new Vector2(moveHorizontal, moveVertical).normalized;
    }

    private void Update()
    {
        Vector2 movementDir = CanMove ? GetMovementDir() : Vector2.zero;
        _dog.MovementDir = movementDir;
            
        if (!CanMove) return;
        
        if (Controller.GetSingleton().GetADown(playerNumber))
        {
            _dog.Roll();
        }
        if (Controller.GetSingleton().GetBDown(playerNumber))
        {
            _dog.Leap();
        }
        if (Controller.GetSingleton().GetYHeld(playerNumber))
        {
            _dog.Pose(true);
        }
    }

}
