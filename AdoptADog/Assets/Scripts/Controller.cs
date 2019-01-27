using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller
{
    public bool enabled { get; set; }
    private static Controller control = null;

    private Controller()
    {
        enabled = true;
    }

    public static Controller getSingleton() {
        if (control == null) {
            control = new Controller();
        }
        return control;
    }

    public float getHorizontal(int player)
    {
        if (!enabled) return 0f;
        
        switch(player)
        {
            case 1:
                return Input.GetAxis("Joystick1Horizontal");
            case 2:
                return Input.GetAxis("Joystick2Horizontal");
            case 3:
                return Input.GetAxis("Joystick3Horizontal");
            case 4:
                return Input.GetAxis("Joystick4Horizontal");
          
        }
        return 0.0f;
    }

    public float getVertical(int player) {
        if (!enabled) return 0f;
        
        switch(player)
        {
            case 1:
                return Input.GetAxis("Joystick1Vertical");
            case 2:
                return Input.GetAxis("Joystick2Vertical");
            case 3:
                return Input.GetAxis("Joystick3Vertical");
            case 4:
                return Input.GetAxis("Joystick4Vertical");           
        }
        return 0.0f;
    }

    public bool getA(int player)
    {
        if (!enabled) return false;
        
        switch(player)
        {
            case 1:
                if (Input.GetKey(KeyCode.Joystick1Button0)) {
                    return true;
                }
                break;
            case 2:
                if (Input.GetKey(KeyCode.Joystick2Button0)) {
                    return true;
                }
                break;
            case 3:
                if (Input.GetKey(KeyCode.Joystick3Button0)) {
                    return true;
                }
                break;
            case 4:
                if (Input.GetKey(KeyCode.Joystick4Button0)) {
                    return true;
                }
                break;               
        }
        return false;
    }

    public bool getB(int player) {
        if (!enabled) return false;
        
        switch(player)
        {
            case 1:
                if (Input.GetKey(KeyCode.Joystick1Button1)) {
                    return true;
                }
                break;
            case 2:
                if (Input.GetKey(KeyCode.Joystick2Button1)) {
                    return true;
                }
                break;
            case 3:
                if (Input.GetKey(KeyCode.Joystick3Button1)) {
                    return true;
                }
                break;
            case 4:
                if (Input.GetKey(KeyCode.Joystick4Button1)) {
                    return true;
                }
                break;               
        }
        return false;
    }

    public bool getX(int player) {
        if (!enabled) return false;
        
        switch(player)
        {
            case 1:
                if (Input.GetKey(KeyCode.Joystick1Button2)) {
                    return true;
                }
                break;
            case 2:
                if (Input.GetKey(KeyCode.Joystick2Button2)) {
                    return true;
                }
                break;
            case 3:
                if (Input.GetKey(KeyCode.Joystick3Button2)) {
                    return true;
                }
                break;
            case 4:
                if (Input.GetKey(KeyCode.Joystick4Button2)) {
                    return true;
                }
                break;               
        }
        return false;
    }

    public bool getY(int player) {
        if (!enabled) return false;
        
        switch(player)
        {
            case 1:
                if (Input.GetKey(KeyCode.Joystick1Button3)) {
                    return true;
                }
                break;
            case 2:
                if (Input.GetKey(KeyCode.Joystick2Button3)) {
                    return true;
                }
                break;
            case 3:
                if (Input.GetKey(KeyCode.Joystick3Button3)) {
                    return true;
                }
                break;
            case 4:
                if (Input.GetKey(KeyCode.Joystick4Button3)) {
                    return true;
                }
                break;               
        }
        return false;
    }
}
