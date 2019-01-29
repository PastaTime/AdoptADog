using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Data.SqlTypes;
using System.Linq;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    private AudioManager _audioManager;
    public static GameManager Instance { get; private set; }

    private void Start()
    {
        _audioManager = GetComponent<AudioManager>();
        Instance = this;
    }

    public void FinishGame(int playerNum)
    {
        _audioManager.BackgroundMusic(false);
        _audioManager.PlayAudio(_audioManager.playerVictory);
        GameState.WinningPlayer = playerNum;
        StartCoroutine(EndRoutine());
    }

    private IEnumerator EndRoutine()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(Constants.EndSceneName);
    }

}
