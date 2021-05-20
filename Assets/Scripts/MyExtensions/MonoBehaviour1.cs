using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonoBehaviour1
{
    public static void StopCoroutine(this MonoBehaviour script, ref Coroutine coroutine)
    {
        if (coroutine != null)
        {
            script.StopCoroutine(coroutine);
            coroutine = null;
        }
    }

    public static void StartCoroutine(this MonoBehaviour script, ref Coroutine coroutine, IEnumerator routine)
    {
        script.StopCoroutine(ref coroutine);
        var startCoroutine = script.StartCoroutine(routine);
        if (startCoroutine != null)
        {
            coroutine = startCoroutine;
        }
    }
}
