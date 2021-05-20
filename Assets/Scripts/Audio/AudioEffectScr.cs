using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEffectScr : AudioSourceScr
{
    public static AudioEffectScr Instance { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
}
