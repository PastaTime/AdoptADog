using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreBar : MonoBehaviour
{
    public int playerNumber = 1;
    
    public Color fillColor = Color.red;
    
    private float _maxPoints;
    public float minXPosition = -6f;
    public float maxXPosition = 6f;

    public Transform bar;

    private PointManager _manager = PointManager.GetSingleton();

    private float _currentPoints;
    
    void Start()
    {
        UpdatePoints(10f);
        bar.GetComponent<SpriteRenderer>().color = fillColor;
        _maxPoints = _manager.WinningPoints;

        _manager.Register(playerNumber, this);
    }

    public void UpdatePoints(float points)
    {
        _currentPoints = points;
        var barFill = _currentPoints / _maxPoints;
        var scale = bar.localScale;
        scale.x = barFill * (maxXPosition - minXPosition);
        bar.localScale = scale;

        var pos = bar.transform.localPosition;
        pos.x = minXPosition + 0.5f * scale.x;
        bar.transform.localPosition = pos;
    }

    public void PlayerWon()
    {
    }
    
    
}
