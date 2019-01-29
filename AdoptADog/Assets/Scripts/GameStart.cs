using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    private AudioManager _manager;
    private ControllerManager _controllerManager;
    private TextMesh _textMesh;
    private static float displayTime = 1.2f;

    // Start is called before the first frame update
    void Start()
    {
        _controllerManager = FindObjectOfType<ControllerManager>();
        _manager = FindObjectOfType<AudioManager>();
        _textMesh = GetComponentInChildren<TextMesh>();
        _controllerManager.Enabled = false;
        _manager.PlayAudio(_manager.gameStart);
        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame() 
    {
        Display("3");
        yield return new WaitForSeconds(displayTime);
        Display("2");
        yield return new WaitForSeconds(displayTime);
        Display("1");
        yield return new WaitForSeconds(displayTime);
        Display("Pose!");
        
        _controllerManager.Enabled = true;
        yield return new WaitForSeconds(displayTime);
    }

    private void Display(string text)
    {
        StartCoroutine(DisplayText(text));
    }

    private IEnumerator DisplayText(string text)
    {
        _textMesh.text = text;
        yield return new WaitForSeconds(displayTime);
        _textMesh.text = "";
    }
    
    
}
