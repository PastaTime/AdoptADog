using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller
{
    public bool Enabled { get; set; }
    private static Controller _control = null;

    private static string[] _joyNames = new string[]
    {
        "Joy1", "Joy2", "Joy3", "Joy4"
    };


    private Controller()
    {
        Enabled = true;
    }

    public static Controller GetSingleton()
    {
        return _control ?? (_control = new Controller());
    }

    public float GetHorizontal(int player)
    {
        if (!Enabled) return 0f;
        
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

    public float GetVertical(int player) {
        if (!Enabled) return 0f;
        
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

    public bool GetADown(int player)
    {
        if (!Enabled) return false;

        if (player < 0 || player > 4) return false;
        
        return Input.GetButtonDown(_joyNames[player - 1] + "A");
    }

    public bool GetBDown(int player) {
        if (!Enabled) return false;
        
        if (player < 0 || player > 4) return false;
        
        return Input.GetButtonDown(_joyNames[player - 1] + "B");
    }

    public bool GetXDown(int player) {
        if (!Enabled) return false;
        
        if (player < 0 || player > 4) return false;
        
        return Input.GetButtonDown(_joyNames[player - 1] + "X");
    }

    public bool GetYDown(int player) {
        if (!Enabled) return false;
        
        if (player < 0 || player > 4) return false;
        
        return Input.GetButtonDown(_joyNames[player - 1] + "Y");
    }
    
    public bool GetYHeld(int player) {
        if (!Enabled) return false;
        
        if (player < 0 || player > 4) return false;
        
        return Input.GetButton(_joyNames[player - 1] + "Y");
    }

    public bool GetBackDown(int player)
    {
        if (!Enabled) return false;
        
        if (player < 0 || player > 4) return false;
        
        return Input.GetButtonDown(_joyNames[player - 1] + "Back");
    }
}
