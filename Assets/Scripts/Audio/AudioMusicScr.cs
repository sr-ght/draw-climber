using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMusicScr : AudioSourceScr
{
    public static AudioMusicScr Instance { get; private set; }

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
        TimeScr.OnChangeTimeScale += TimeScr_OnChangeTimeScale;
    }

    protected override void OnDestroy()
    {
        base.Awake();
        TimeScr.OnChangeTimeScale -= TimeScr_OnChangeTimeScale;
    }

    private void TimeScr_OnChangeTimeScale(float scale)
    {
        _audioSource.pitch = Mathf.Lerp(0.95f, 1f, scale);
        _audioSource.volume = Mathf.Lerp(0.2f, 0.5f, scale);
    }
}
