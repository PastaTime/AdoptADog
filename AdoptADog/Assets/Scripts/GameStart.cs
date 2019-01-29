using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameStart : MonoBehaviour
{

    private AudioManager manager;
    private static float displayTime = 1.2f;

    private TextMesh textMesh;
    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<AudioManager>();
        textMesh = GetComponentInChildren<TextMesh>();
        Controller.GetSingleton().Enabled = false;
        manager.PlayAudio(manager.gameStart);
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
        Controller.GetSingleton().Enabled = true;
        yield return new WaitForSeconds(displayTime);
    }

    private void Display(string text)
    {
        StartCoroutine(DisplayText(text));
    }

    private IEnumerator DisplayText(string text)
    {
        textMesh.text = text;
        yield return new WaitForSeconds(displayTime);
        textMesh.text = "";
    }
    
    
}
