using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonToggleScr : MonoBehaviour
{
    public event Action<bool> OnChangeButtonState;

    [SerializeField] protected bool _originalState;

    protected bool _state;
    public virtual bool State
    {
        get => _state;
        set
        {
            _state = value;
            OnChangeButtonState?.Invoke(_state);
        }
    }

    protected void Start()
    {
        State = _originalState;
    }

    public void ChangeState() => State = !State;
}
