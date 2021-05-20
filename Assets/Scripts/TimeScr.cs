using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScr : MonoBehaviour
{
    public static event Action<float> OnChangeTimeScale;

    [SerializeField, Range(0.005f, 0.02f)] private float _fixedTimestep = 0.01f;

    public float TimeScale
    {
        get => Time.timeScale;
        set
        {
            Time.timeScale = value;
            OnChangeTimeScale?.Invoke(value);
        }
    }

    private void OnValidate()
    {
        UpdateFixedDeltaTime();
    }

    private void OnEnable()
    {
        DrawArmScr.OnChangeDrawState += DrawArmScr_OnChangeDrawState;
    }

    private void OnDisable()
    {
        DrawArmScr.OnChangeDrawState -= DrawArmScr_OnChangeDrawState;
    }

    private void DrawArmScr_OnChangeDrawState(bool value)
    {
        TimeScale = value ? 0.25f : 1f;
        UpdateFixedDeltaTime();
    }

    private void UpdateFixedDeltaTime()
    {
        Time.fixedDeltaTime = TimeScale * _fixedTimestep;
    }
}
