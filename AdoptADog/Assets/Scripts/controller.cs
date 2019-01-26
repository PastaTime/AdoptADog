using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controller : MonoBehaviour
{
    public float speed = 10.0F;
    public float rotationSpeed = 10.0F;
    // Update is called once per frame
    void Update()
    {
        float test = Input.GetAxis("Vertical");
        float testHorizontal = Input.GetAxis("Horizontal");
        Debug.Log("v " + test + "h " + testHorizontal);
        float translationY = Input.GetAxis("Vertical") * speed;
        //Debug.Log("translation " + Input.GetAxis("Vertical"));
        float translateX = Input.GetAxis("Horizontal") * rotationSpeed;
        translationY *= Time.deltaTime;
        translateX *= Time.deltaTime;
        transform.Translate(0, 0, translationY);
        transform.Translate(translateX, 0, 0);

        if(Input.GetKeyDown(KeyCode.Joystick1Button0)) {
            Debug.Log("A");
        }
                if(Input.GetKeyDown(KeyCode.Joystick1Button1)) {
            Debug.Log("B");
        }
                if(Input.GetKeyDown(KeyCode.Joystick1Button2)) {
            Debug.Log("C");
        }
                if(Input.GetKeyDown(KeyCode.Joystick1Button3)) {
            Debug.Log("D");
        }
                if(Input.GetKeyDown(KeyCode.Joystick1Button4)) {
            Debug.Log("E");
        }
                if(Input.GetKeyDown(KeyCode.Joystick1Button5)) {
            Debug.Log("F");
        }
                if(Input.GetKeyDown(KeyCode.Joystick1Button6)) {
            Debug.Log("G");
        }
                if(Input.GetKeyDown(KeyCode.Joystick1Button7)) {
            Debug.Log("H");
        }
                if(Input.GetKeyDown(KeyCode.Joystick1Button8)) {
            Debug.Log("I");
        }
        if(Input.GetKeyDown(KeyCode.Joystick1Button9)) {
            Debug.Log("J");
        }
                if(Input.GetKeyDown(KeyCode.Joystick1Button10)) {
            Debug.Log("K");
        }        if(Input.GetKeyDown(KeyCode.Joystick1Button11)) {
            Debug.Log("L");
        }        if(Input.GetKeyDown(KeyCode.Joystick1Button12)) {
            Debug.Log("M");
        }        if(Input.GetKeyDown(KeyCode.Joystick1Button13)) {
            Debug.Log("N");
        }
                if(Input.GetKeyDown(KeyCode.Joystick1Button14)) {
            Debug.Log("O");
        }        if(Input.GetKeyDown(KeyCode.Joystick1Button15)) {
            Debug.Log("P");
        }        if(Input.GetKeyDown(KeyCode.Joystick1Button16)) {
            Debug.Log("Q");
        }
                if(Input.GetKeyDown(KeyCode.Joystick1Button17)) {
            Debug.Log("R");
        }
                if(Input.GetKeyDown(KeyCode.Joystick1Button18)) {
            Debug.Log("S");
        }
    }
}
