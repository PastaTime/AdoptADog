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

    private void Start()
    {
        _dog = GetComponent<Dog>();
        _dog.PlayerNumber = playerNumber;
    }

    private Vector2 GetMovementDir()
    {
        float moveHorizontal = Controller.getSingleton().getHorizontal(playerNumber);
        float moveVertical = Controller.getSingleton().getVertical(playerNumber);

        return new Vector2(moveHorizontal, moveVertical).normalized;
    }

    private void Update()
    {
        Vector2 movementDir = GetMovementDir();
        _dog.MovementDir = movementDir;

        if (Controller.getSingleton().getA(playerNumber))
        {
            Debug.Log("Roll");
            _dog.Roll();
        }
        if (Controller.getSingleton().getB(playerNumber))
        {
            _dog.Leap();
        }
    }

}
