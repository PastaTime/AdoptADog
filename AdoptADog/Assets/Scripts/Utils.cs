using UnityEngine;
using Random = System.Random;

public static class Utils
{
    private static readonly Random Random = new Random();

    public static Vector2 RandomPoint(Vector2 position, Rect rect)
    {
        float maxX = position.x + rect.width/2 - 0.25f - 0.5f;
        float maxY = position.y + rect.height/2 - 0.25f - 0.5f;
        float minX = position.x - rect.width/2 + 0.25f + 0.5f;
        float minY = position.y - rect.height/2 + 0.25f + 0.5f;

        var randomX =  (float)Random.Next((int)minX, (int)maxX);
        var randomY = (float)Random.Next((int)minY, (int)maxY);
        return new Vector2(randomX, randomY);
    }
}