using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightScript : MonoBehaviour
{
    
    public float radius = 1.5f;

    public RectTransform _playField;
    private float maxX;
    private float maxY;
    private float minX;
    private float minY;
    private float xPoint;
    private float yPoint;

    void Start()
    {
        //don't know if any scaling is required
        _playField = GameObject.Find("Grid").GetComponent<RectTransform>();
        maxX = _playField.position.x + _playField.rect.width - 0.5f - radius;
        maxY = _playField.position.y + _playField.rect.length - 0.5f - radius;
        minX = _playField.position.x + 0.5f + radius;
        minY = _playField.position.y + 0.5f + radius;

        Vector2 min = new Vector2(minX, minY);
        
        
    }

    void Update()
    {

    }

    public bool InSpotlight(string player) {
        Collider2D[] colliding = Physics2D.OverlapCircleAll(transform.position, radius);
        for (int i = 0; i < colliding.Length; i++) {
            if (colliding[i].gameObject.name == player) {
                return true;
            }
        }
        return false;
    }
}
