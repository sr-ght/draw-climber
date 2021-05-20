using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public static Main Instance { get; private set; }
    public static event Action<State> OnChangeState;

    [SerializeField] private State _startState;

    private State _state;
    public State State
    {
        get => _state;
        set
        {
            _state = value;
            //Debug.Log(_state.ToString());
            OnChangeState?.Invoke(_state);
            if (_state == State.NextLevel)
            {
                State = State.Menu;
            }
        }
    }

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        State = _startState;
    }

    public void SetState(string stateName) => State = (State)Enum.Parse(typeof(State), stateName);
}
