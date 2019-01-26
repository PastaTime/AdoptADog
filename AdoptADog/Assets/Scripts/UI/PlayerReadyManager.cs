using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReadyManager : MonoBehaviour
{
    public List<PlayerReadyButton> buttonList = new List<PlayerReadyButton>();

    void Update()
    {
        if (AllPlayersReady())
        {
            StartGame();
        }
        
    }

    public void Register(PlayerReadyButton button)
    {
        buttonList.Add(button);
    }

    private bool AllPlayersReady()
    {
        foreach (PlayerReadyButton button in buttonList)
        {
            if (!button.PlayerReady())
            {
                return false;
            }
        }
        
        return true;
    }
    

    private void StartGame()
    {
        // Transition to next scene
        Debug.Log("start game!");
    }
    
}
