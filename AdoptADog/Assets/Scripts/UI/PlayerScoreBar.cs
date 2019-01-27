using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

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
    public int playerNumber = 1;
    
    public Color fillColor = Color.red;
    public Color flashingColor = Color.white;
    
    private float _maxPoints;
    public float minXPosition = -6f;
    public float maxXPosition = 6f;
    private float _flashTime = 0.25f;

    public Transform barFill;
    public Transform barFrame;
    public Transform dropInButton;

    private PointManager _manager = PointManager.GetSingleton();

    private float _currentPoints;
    private GameManager _gameManager;
    
    void Start()
    {
        barFill.GetComponent<SpriteRenderer>().color = fillColor;
        _maxPoints = _manager.WinningPoints;

        _manager.Register(playerNumber, this);
        UpdatePoints(0.0f);

    }

    void Update()
    {
        if (_maxPoints * .75 < _currentPoints) 
        {
            ToggleColor();
        }
        
        Enable = _gameManager.GetActivePlayers().Contains(playerNumber);
        
        if (Controller.GetSingleton().GetBackDown(playerNumber))
        {
            Enable = false;
            _gameManager.DespawnPlayer(playerNumber);
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
        _gameManager = FindObjectOfType <GameManager>();
        if (_gameManager != null)
        {
            Enable = _gameManager.GetActivePlayers().Contains(playerNumber);
            Debug.Log("Setting Player " + playerNumber + " Score Bar on:" + _enable);
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
