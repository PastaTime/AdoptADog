﻿using System.Collections;
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
    private float freezeTime = 0.0f;

    void Start()
    {
        //don't know if any scaling is required
        var position = _playField.position;
        var rect = _playField.rect;
        maxX = position.x + rect.width/2 - 0.25f - radius;
        maxY = position.y + rect.height/2 - 0.25f - radius;
        minX = position.x - rect.width/2 + 0.25f + radius;
        minY = position.y - rect.height/2 + 0.25f + radius;

        newPosition();
    }

    void Update()
    {
        if (transform.position.x == nextPosition.x && transform.position.y == nextPosition.y) {
            if (freezeTime <= 0) {
                newPosition();
                System.Random rnd = new System.Random();
                freezeTime = (float)rnd.Next(0,25)/10;
            } else {
                freezeTime -= Time.deltaTime;
            }
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
