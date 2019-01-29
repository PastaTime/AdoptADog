using UnityEngine;
using UnityEngine.Serialization;
﻿using DefaultNamespace;
using UnityEngine.UI;
using XInputDotNetPure;

[RequireComponent(typeof(PulseButton))]
public class PlayerReadyButton : MonoBehaviour
{
    public AudioManager _audioManager;
    public bool buttonSelected = false;
    public Dog dog;

    public PlayerIndex playerNumber;

    // Used for flashing the button.
    private PlayerReadyManager _manager;
    private ControllerManager _controllerManager;
    private PulseButton _pulseButton;

    public bool PlayerReady => buttonSelected;

    void Start()
    {
        _audioManager = FindObjectOfType<AudioManager>();
        _manager = FindObjectOfType<PlayerReadyManager>();
        _controllerManager = FindObjectOfType<ControllerManager>();
        
        _pulseButton = GetComponent<PulseButton>();
        

        _manager.Register(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonSelected)
        {
            dog.Pose();
            return;
        }

        if (_controllerManager.GetADown(playerNumber))
        {
            SelectButton();
        }
    }


    private void SelectButton()
    {
        _audioManager.PlayAudio(_audioManager.playerReady);
        GameState.ActivePlayers.Add(playerNumber);
        _pulseButton.Selected = true;
        buttonSelected = true;
    }
}
