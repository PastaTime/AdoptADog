using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using XInputDotNetPure;

[RequireComponent(typeof(Dog))]
public class PlayerController : MonoBehaviour
{
    public PlayerIndex playerNumber = PlayerIndex.One;
    private Dog _dog;

    public bool CanMove { get; set; } = true;

    private ControllerManager _controllerManager;

    private void Start()
    {
        _dog = GetComponent<Dog>();
        _dog.PlayerNumber = playerNumber;
        _controllerManager = FindObjectOfType<ControllerManager>();
    }

    private Vector2 GetMovementDir()
    {
        float moveHorizontal = _controllerManager.GetHorizontal(playerNumber);
        float moveVertical = _controllerManager.GetVertical(playerNumber);

        return new Vector2(moveHorizontal, moveVertical).normalized;
    }

    private void Update()
    {
        Vector2 movementDir = CanMove ? GetMovementDir() : Vector2.zero;
        _dog.MovementDir = movementDir;
            
        if (!CanMove) return;
        
        if (_controllerManager.GetADown(playerNumber))
        {
            _dog.DoAction(Dog.Roll);
        }
        if (_controllerManager.GetBDown(playerNumber))
        {
            _dog.DoAction(Dog.Leap);
        }
        if (_controllerManager.GetYHeld(playerNumber))
        {
            _dog.Pose(true);
        }
    }

}
