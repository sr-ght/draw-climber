using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitmarkerScr : MonoBehaviour
{
    [SerializeField] private Transform _hitmarker;
    [SerializeField] private float _timeShow;
    [SerializeField] private AudioClip _audioClip;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AudioEffectScr.Instance.PlayOneShot(_audioClip);
            _hitmarker.position = (Vector2)Input.mousePosition;
            _hitmarker.gameObject.SetActive(true);
        }
        if (Input.GetMouseButtonUp(0))
        {
            StopAllCoroutines();
            StartCoroutine(Co_Activate());
        }
    }

    private IEnumerator Co_Activate()
    {
        yield return new WaitForSeconds(_timeShow);
        _hitmarker.gameObject.SetActive(false);
    }
}
