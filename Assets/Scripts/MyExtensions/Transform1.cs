using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Transform1
{
    public static void SetLocalPositionAndRotation(this Transform transform, Vector3 position, Quaternion rotation)
    {
        transform.localPosition = position;
        transform.localRotation = rotation;
    }
}
