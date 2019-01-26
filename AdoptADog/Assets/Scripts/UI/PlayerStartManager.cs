using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStartManager : MonoBehaviour
{
    public List<PlayerStartButton> buttonList = new List<PlayerStartButton>();

    void Update()
    {
        if (AllPlayersReady())
        {
            StartGame();
        }
        
    }

    public void Register(PlayerStartButton button)
    {
        buttonList.Add(button);
    }

    private bool AllPlayersReady()
    {
        foreach (PlayerStartButton button in buttonList)
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
        Debug.Log("start game!");
        Console.WriteLine("START GAME!");
    }
    
}
