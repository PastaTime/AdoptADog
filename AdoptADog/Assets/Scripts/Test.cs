using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        var controller = Controller.GetSingleton();
        for (int i = 1; i <= 4; i++)
        {
            Debug.Log("Player " + i + ": " + Input.GetButtonDown("Joy" + i + "A"));
        }
    }
}
