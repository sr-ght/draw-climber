using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class AudioSourceScr : MonoBehaviour
{
    [SerializeField] protected AudioSource _audioSource;
    [SerializeField] protected ButtonToggleScr _buttonToggle;

    private Coroutine _coroutineWait;

    public bool Mute
    {
        get => _audioSource.mute;
        set
        {
            _audioSource.mute = value;
        }
    }

    protected virtual void Awake() =>
        _buttonToggle.OnChangeButtonState += _buttonToggle_OnChangeButtonState;

    protected virtual void OnDestroy() =>
        _buttonToggle.OnChangeButtonState -= _buttonToggle_OnChangeButtonState;

    public void PlayOneShot(AudioClip audioClip)
    {
        if (audioClip == null)
            return;

        _audioSource.PlayOneShot(audioClip);
    }

    public void PlayIfFree(AudioClip audioClip)
    {
        if (_audioSource.clip)
            return;

        Play(audioClip);
    }

    private void Play(AudioClip audioClip)
    {
        if (audioClip == null)
            return;

        _audioSource.clip = audioClip;
        _audioSource.Play();
        this.StartCoroutine(ref _coroutineWait, Co_WaitTime(audioClip.length));
    }

    private IEnumerator Co_WaitTime(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        _audioSource.Stop();
        _audioSource.clip = null;
    }

    private void _buttonToggle_OnChangeButtonState(bool mute) => Mute = mute;
}
