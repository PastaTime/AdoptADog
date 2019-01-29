using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    private AudioManager _manager;
    private ControllerManager _controllerManager;
    private TextMesh _textMesh;
    
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
        yield return new WaitForSeconds(1.0f);
        Display("2");
        yield return new WaitForSeconds(1.0f);
        Display("1");
        yield return new WaitForSeconds(1.0f);
        Display("Pose!");
        
        _controllerManager.Enabled = true;
        yield return new WaitForSeconds(1.0f);
    }

    private void Display(string text)
    {
        StartCoroutine(DisplayText(text));
    }

    private IEnumerator DisplayText(string text)
    {
        _textMesh.text = text;
        yield return new WaitForSeconds(1f);
        _textMesh.text = "";
    }
    
    
}
