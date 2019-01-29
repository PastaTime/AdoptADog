using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using XInputDotNetPure;

public class PlayerScoreBar : MonoBehaviour
{
    private bool _enable = true;
    
    public bool Enable
    {
        get => _enable;
        set
        {
            _enable = value;
            if (_enable)
            {
                barFrame.gameObject.SetActive(true);
                dropInButton.gameObject.SetActive(false);
            }
            else
            {
                barFrame.gameObject.SetActive(false);
                dropInButton.gameObject.SetActive(true);
            }
        }
    }

    public PlayerIndex playerIndex = PlayerIndex.One;
    
    public Color fillColor = Color.red;
    public Color flashingColor = Color.white;
    
    private float _maxPoints;
    public float minXPosition = -6f;
    public float maxXPosition = 6f;
    private float _flashTime = 0.25f;

    public Transform barFill;
    public Transform barFrame;
    public Transform dropInButton;

    

    private float _currentPoints;
    private PointManager _manager;
    private PlayerManager _playerManager;
    private ControllerManager _controllerManager;
    
    void Start()
    {
        _controllerManager = FindObjectOfType<ControllerManager>();
        _manager = FindObjectOfType<PointManager>();
        
        barFill.GetComponent<SpriteRenderer>().color = fillColor;
        _maxPoints = _manager.WinningPoints;

        _manager.Register(playerIndex, this);
        Enable = true;
        UpdatePoints(0.0f);
    }

    void Update()
    {
        if (_maxPoints * .75 < _currentPoints) 
        {
            ToggleColor();
        }
        
        
        if (_controllerManager.GetBackDown(playerIndex))
        {
            Enable = false;
            _playerManager.DespawnPlayer(playerIndex);
        }
    }

    private void ToggleColor() 
    {
        _flashTime -= Time.deltaTime;
        if (_flashTime < 0) 
        {
            if (barFill.GetComponent<SpriteRenderer>().color == fillColor) 
            {
                barFill.GetComponent<SpriteRenderer>().color = flashingColor;
            } else 
            {
                barFill.GetComponent<SpriteRenderer>().color = fillColor;
            }
            _flashTime = 0.25f;
        }
    }

    void Awake()
    {
        _playerManager = FindObjectOfType<PlayerManager>();
        if (_playerManager != null)
        {
            Enable = true;
            Debug.Log("Setting Player " + playerIndex + " Score Bar on:" + _enable);
        }
    }

    public void UpdatePoints(float points)
    {
        if (!Enable) return;
        
        _currentPoints = points;
        var fill = _currentPoints / _maxPoints;
        var scale = this.barFill.localScale;
        scale.x = fill * (maxXPosition - minXPosition);
        this.barFill.localScale = scale;

        var transform1 = this.barFill.transform;
        var pos = transform1.localPosition;
        pos.x = minXPosition + 0.5f * scale.x;
        transform1.localPosition = pos;
    }

    public void PlayerWon()
    {
    }
}
