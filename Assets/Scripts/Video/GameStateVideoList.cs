using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameStateVideoList : VideoList
{
    [PropertyOrder(-1)]
    [SerializeField] private State _state = State.Start;

    public State State { get => _state; }
}
