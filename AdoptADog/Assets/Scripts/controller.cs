using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controller : MonoBehaviour
{
    public float speed = 5.0F;
    public float rotationSpeed = 10.0F;
    // Update is called once per frame
    void Update()
    {
        float test = Input.GetAxis("Vertical");
        float testHorizontal = Input.GetAxis("Horizontal");
        float translationY = Input.GetAxis("Vertical") * speed;
        //Debug.Log("translation " + Input.GetAxis("Vertical"));
        float translateX = Input.GetAxis("Horizontal") * rotationSpeed;
        translationY *= Time.deltaTime;
        translateX *= Time.deltaTime;
        transform.Translate(0, 0, translationY);
        transform.Translate(translateX, 0, 0);

        if (Input.GetKey(KeyCode.JoystickButton0))
        {
            Debug.Log("A"); // A
        }
        if (Input.GetKey(KeyCode.JoystickButton1))
        {
            Debug.Log("B"); // B
        }
        if (Input.GetKey(KeyCode.JoystickButton2))
        {
            Debug.Log("C");
        }
        if (Input.GetKey(KeyCode.JoystickButton3))
        {
            Debug.Log("D");
        }
        if (Input.GetKey(KeyCode.JoystickButton4))
        {
            Debug.Log("E");
        }
        if (Input.GetKey(KeyCode.JoystickButton5))
        {
            Debug.Log("F");
        }
        if (Input.GetKey(KeyCode.JoystickButton6))
        {
            Debug.Log("G");
        }
        if (Input.GetKey(KeyCode.JoystickButton7))
        {
            Debug.Log("H");
        }
        if (Input.GetKey(KeyCode.JoystickButton8))
        {
            Debug.Log("I");
        }
        if (Input.GetKey(KeyCode.JoystickButton9))
        {
            Debug.Log("J");
        }
        if (Input.GetKey(KeyCode.JoystickButton10))
        {
            Debug.Log("K");
        }
        if (Input.GetKey(KeyCode.JoystickButton11))
        {
            Debug.Log("L");
        }
        if (Input.GetKey(KeyCode.JoystickButton12))
        {
            Debug.Log("M");
        }
        if (Input.GetKey(KeyCode.JoystickButton13))
        {
            Debug.Log("N");
        }
        if (Input.GetKey(KeyCode.JoystickButton14))
        {
            Debug.Log("O");
        }
        if (Input.GetKey(KeyCode.JoystickButton15))
        {
            Debug.Log("P");
        }
        if (Input.GetKey(KeyCode.JoystickButton16))
        {
            Debug.Log("Q");
        }
        if (Input.GetKey(KeyCode.JoystickButton17))
        {
            Debug.Log("R");
        }
        if (Input.GetKey(KeyCode.JoystickButton18))
        {
            Debug.Log("S");
        }
    }
}
