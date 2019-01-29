using System;
using UnityEngine;
using XInputDotNetPure;

public class ControllerManager : MonoBehaviour
{
    public bool Enabled { get; set; }

    private GamePadButtons[] lastFrame = new GamePadButtons[4];

    void Start()
    {
        DontDestroyOnLoad(this);
        Enabled = true;
    }

    void LateUpdate()
    {
        for (int i = 0; i < 4; i++)
        {
            lastFrame[i] = GamePad.GetState((PlayerIndex) i).Buttons;
        }
    }

    public float GetHorizontal(PlayerIndex player)
    {
        if (!Enabled) return 0f;

        if (player == PlayerIndex.One && GetAxisFromKeys(KeyCode.RightArrow, KeyCode.LeftArrow) != 0)
        {
            return (float) GetAxisFromKeys(KeyCode.RightArrow, KeyCode.LeftArrow);
        }
        
        if (player == PlayerIndex.Two && GetAxisFromKeys(KeyCode.D, KeyCode.A) != 0)
        {
            return (float) GetAxisFromKeys(KeyCode.D, KeyCode.A);
        }

        GamePadState state = GamePad.GetState(player);
    
        return state.ThumbSticks.Left.X + state.ThumbSticks.Right.X;
    }

    public float GetVertical(PlayerIndex player) {
        if (!Enabled) return 0f;
        
        if (player == PlayerIndex.One && GetAxisFromKeys(KeyCode.UpArrow, KeyCode.DownArrow) != 0)
        {
            return (float) GetAxisFromKeys(KeyCode.UpArrow, KeyCode.DownArrow);
        }
        
        if (player == PlayerIndex.Two && GetAxisFromKeys(KeyCode.W, KeyCode.S) != 0)
        {
            return (float) GetAxisFromKeys(KeyCode.W, KeyCode.S);
        }
        
        GamePadState state = GamePad.GetState(player);
        
        return state.ThumbSticks.Left.Y + state.ThumbSticks.Right.Y;
    }

    public bool GetADown(PlayerIndex player)
    {
        if (!Enabled) return false;

        if (player == PlayerIndex.One && Input.GetKeyDown(KeyCode.Comma))
        {
            return true;
        }
        
        if (player == PlayerIndex.Two && Input.GetKeyDown(KeyCode.C))
        {
            return true;
        }

        GamePadState state = GamePad.GetState(player);

        return state.Buttons.A == ButtonState.Pressed && lastFrame[(int) player].A == ButtonState.Released;
    }

    public bool GetBDown(PlayerIndex player) {
        if (!Enabled) return false;
        
        if (player == PlayerIndex.One && Input.GetKeyDown(KeyCode.Period))
        {
            return true;
        }
        
        if (player == PlayerIndex.Two && Input.GetKeyDown(KeyCode.V))
        {
            return true;
        }

        GamePadState state = GamePad.GetState(player);

        return state.Buttons.B == ButtonState.Pressed && lastFrame[(int) player].B == ButtonState.Released;
    }

    public bool GetXDown(PlayerIndex player) {
        if (!Enabled) return false;
        
        if ((player == PlayerIndex.One || player == PlayerIndex.Two) && Input.GetKeyDown(KeyCode.Return))
        {
            return true;
        }
        
        GamePadState state = GamePad.GetState(player);

        return state.Buttons.X == ButtonState.Pressed && lastFrame[(int) player].X == ButtonState.Released;
    }

    public bool GetYDown(PlayerIndex player) {
        if (!Enabled) return false;

        if (player == PlayerIndex.One && Input.GetKeyDown(KeyCode.Slash))
        {
            return true;
        }
        
        if (player == PlayerIndex.Two && Input.GetKeyDown(KeyCode.B))
        {
            return true;
        }
        
        GamePadState state = GamePad.GetState(player);

        return state.Buttons.Y == ButtonState.Pressed && lastFrame[(int) player].Y == ButtonState.Released;
    }
    
    public bool GetYHeld(PlayerIndex player) {
        if (!Enabled) return false;

        GamePadState state = GamePad.GetState(player);
        
        if (player == PlayerIndex.One && Input.GetKey(KeyCode.Slash))
        {
            return true;
        }
        
        if (player == PlayerIndex.Two && Input.GetKey(KeyCode.B))
        {
            return true;
        }

        return state.Buttons.Y == ButtonState.Pressed;
    }

    public bool GetBackDown(PlayerIndex player)
    {
        if (!Enabled) return false;

        GamePadState state = GamePad.GetState(player);

        return state.Buttons.Back == ButtonState.Pressed && lastFrame[(int) player].Back == ButtonState.Released;
    }

    private int GetAxisFromKeys(KeyCode positiveKey, KeyCode negativeKey)
    {
        var axis = Input.GetKey(positiveKey) ? 1 : 0;
        axis += Input.GetKey(negativeKey) ? -1 : 0;

        return axis;
    }
}
