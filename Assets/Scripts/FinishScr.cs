using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishScr : MonoBehaviour
{
    [SerializeField] private GameObject[] _particlesObjects;

    private void OnEnable()
    {
        Main.OnChangeState += Main_OnChangeState;
    }

    private void OnDisable()
    {
        Main.OnChangeState -= Main_OnChangeState;
    }

    private void Main_OnChangeState(State state)
    {
        var isEnable = state == State.Finish;
        foreach (var o in _particlesObjects)
        {
            o?.SetActive(isEnable);
        }
    }
}
