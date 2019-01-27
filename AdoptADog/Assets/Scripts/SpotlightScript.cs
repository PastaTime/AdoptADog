using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpotlightScript : MonoBehaviour
{
    
    public float radius = 1.5f;

    public RectTransform _playField;
    float maxX;
    float maxY;
    float minX;
    float minY;

    private Vector3 nextPosition;
    private Vector3 currentPosition;
    private float distance = 0.01f;

    void Start()
    {
        //don't know if any scaling is required
        maxX = _playField.position.x + _playField.rect.width - 0.25f - radius;
        maxY = _playField.position.y + _playField.rect.height - 0.25f - radius;
        minX = _playField.position.x + 0.25f + radius;
        minY = _playField.position.y + 0.25f + radius;

        newPosition();
    }

    void Update()
    {
        if (transform.position.x == nextPosition.x && transform.position.y == nextPosition.y) {
            newPosition();
        } else {
            distance += 0.01f;
            transform.position = Vector3.Lerp(currentPosition, nextPosition, distance);
        }

    }

    private void newPosition() {
        distance = 0.01f;
        System.Random rnd = new System.Random();
        float randomX =  (float)rnd.Next((int)minX, (int)maxX);
        float randomY = (float)rnd.Next((int)minY, (int)maxY);
        nextPosition = new Vector3(randomX, randomY, 0);
        currentPosition = transform.position;
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
