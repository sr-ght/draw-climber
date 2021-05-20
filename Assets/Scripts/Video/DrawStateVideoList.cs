using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DrawStateVideoList : VideoList
{
    [PropertyOrder(-1)]
    [SerializeField] private bool _isStartDraw;

    public bool IsStartDraw { get => _isStartDraw; }
}
