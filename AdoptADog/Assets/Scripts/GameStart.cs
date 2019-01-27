using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{

    private AudioManager manager;

    private TextMesh textMesh;
    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<AudioManager>();
        textMesh = GetComponentInChildren<TextMesh>();
        Controller.getSingleton().enabled = false;
        manager.PlayAudio(manager.gameStart);
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
        Controller.getSingleton().enabled = true;
        yield return new WaitForSeconds(1.0f);
    }

    private void Display(string text)
    {
        StartCoroutine(DisplayText(text));
    }

    private IEnumerator DisplayText(string text)
    {
        textMesh.text = text;
        yield return new WaitForSeconds(1f);
        textMesh.text = "";
    }
    
    
}
