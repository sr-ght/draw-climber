using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CollisionDetectorScr : MonoBehaviour
{
    public event Action<object> OnCollisionStay;

    private void OnCollisionStay2D(Collision2D collision)
    {
        OnCollisionStay?.Invoke(collision);
    }
}
