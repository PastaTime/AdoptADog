using UnityEngine;
using UnityEngine.Serialization;
﻿using DefaultNamespace;
using UnityEngine.UI;

[RequireComponent(typeof(PulseButton))]
public class PlayerReadyButton : MonoBehaviour
{
    public AudioManager _audioManager;
    public bool buttonSelected = false;
    public Dog dog;

    public int playerNumber;

    // Used for flashing the button.
    private PlayerReadyManager _manager;
    private PulseButton _pulseButton;

    public bool PlayerReady => buttonSelected;

    void Start()
    {
        _audioManager = FindObjectOfType<AudioManager>();
        _manager = FindObjectOfType<PlayerReadyManager>();
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

        if (Controller.GetSingleton().GetADown(playerNumber))
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
