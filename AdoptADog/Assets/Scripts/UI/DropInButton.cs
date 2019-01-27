using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Security.Cryptography.X509Certificates;
using DefaultNamespace;
using UnityEngine;

public class DropInButton : MonoBehaviour
{
    public int playerNumber = 1;
    public PlayerScoreBar scoreBar;

    private GameManager _manager;

    void Start()
    {
        scoreBar = GetComponentInParent<PlayerScoreBar>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Controller.GetSingleton().GetADown(playerNumber))
        {
            return;
        }
    
        scoreBar.Enable = true;
        Debug.Log("Player " + playerNumber + " Dropping In!");
        _manager.SpawnPlayer(playerNumber);
    }
}
