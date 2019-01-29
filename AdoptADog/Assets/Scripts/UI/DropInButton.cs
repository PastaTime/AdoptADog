using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Security.Cryptography.X509Certificates;
using DefaultNamespace;
using UnityEngine;
using XInputDotNetPure;

public class DropInButton : MonoBehaviour
{
    public PlayerIndex player = PlayerIndex.One;
    public PlayerScoreBar scoreBar;

    private PlayerManager _playerManager;
    private ControllerManager _controllerManager;

    void Start()
    {
        scoreBar = GetComponentInParent<PlayerScoreBar>();
        _playerManager = FindObjectOfType<PlayerManager>();
        _controllerManager = FindObjectOfType<ControllerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_controllerManager.GetADown(player))
        {
            return;
        }
    
        scoreBar.Enable = true;
        Debug.Log("Player " + player + " Dropping In!");
        _playerManager.SpawnPlayer(player);
    }
}
