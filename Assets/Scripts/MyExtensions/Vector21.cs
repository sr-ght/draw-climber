using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector21
{
    public static float Atan2(this Vector2 vector) => Mathf.Atan2(vector.y, vector.x);
    public static float GetLineLength(this Vector2[] vertices)
    {
        if (vertices == null || vertices.Length < 2)
        {
            return 0f;
        }
        var sum = 0f;
        var count = vertices.Length - 1;
        for (int i = 0; i < count; i++)
        {
            sum += Vector2.Distance(vertices[i], vertices[i + 1]);
        }
        return sum;
    }
}
