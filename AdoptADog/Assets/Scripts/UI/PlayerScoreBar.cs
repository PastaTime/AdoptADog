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
    
    private float _maxPoints;
    public float minXPosition = -6f;
    public float maxXPosition = 6f;

    public Transform barFill;
    public Transform barFrame;
    public Transform dropInButton;

    private PointManager _manager = PointManager.GetSingleton();
    private AudioManager _audioManager;
    
    private float _currentPoints;
    private GameManager _gameManager;
    
    void Start()
    {
        UpdatePoints(10f);
        barFill.GetComponent<SpriteRenderer>().color = fillColor;
        _maxPoints = _manager.WinningPoints;
        _audioManager = FindObjectOfType<AudioManager>();
        
        _manager.Register(playerNumber, this);
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

    private void Update()
    {
        if (Controller.GetSingleton().GetBackDown(playerNumber))
        {
            Enable = false;
            _gameManager.DespawnPlayer(playerNumber);
        }
    }

    public void UpdatePoints(float points)
    {
        _currentPoints = points;
        var barFill = _currentPoints / _maxPoints;
        var scale = this.barFill.localScale;
        scale.x = barFill * (maxXPosition - minXPosition);
        this.barFill.localScale = scale;

        var pos = this.barFill.transform.localPosition;
        pos.x = minXPosition + 0.5f * scale.x;
        this.barFill.transform.localPosition = pos;
    }

    public void PlayerWon()
    {
        _audioManager.PlayAudio(_audioManager.playerVictory);
    }
}
