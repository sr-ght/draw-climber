using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class СomplimentTextScr : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Animator _animator;
    [SerializeField] private ComplimentTextClip[] _compliments;

    private void OnEnable()
    {
        _text.text = "";
        DrawArmScr.OnChangeDrawState += DrawArmScr_OnChangeDrawState;
    }

    private void OnDisable()
    {
        DrawArmScr.OnChangeDrawState -= DrawArmScr_OnChangeDrawState;
    }

    private void DrawArmScr_OnChangeDrawState(bool value)
    {
        if (!value)
        {
            _animator.SetTrigger(Constants.ANIMATOR_Show);
            if (_compliments != null && _compliments.Length > 0)
            {
                var freeCompliments = _compliments.Where(c => c.Text != _text.text).ToArray();
                if (freeCompliments != null && freeCompliments.Length != 0)
                {
                    var curComplement = freeCompliments[Random.Range(0, freeCompliments.Length)];
                    _text.text = curComplement.Text;
                    AudioEffectScr.Instance.PlayIfFree(curComplement.AudioClip);
                }
            }
        }
    }

    [System.Serializable]
    private class ComplimentTextClip
    {
        [SerializeField] private string _text;
        [SerializeField] private AudioClip _audioClip;

        public string Text => _text;
        public AudioClip AudioClip => _audioClip;
    }
}
