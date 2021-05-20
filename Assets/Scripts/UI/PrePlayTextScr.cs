using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PrePlayTextScr : MonoBehaviour
{
    [SerializeField] private Animator _animatorText;
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private string[] _texts;

    private Coroutine _coroutinePrePlay;

    private void Awake()
    {
        Main.OnChangeState += Main_OnChangeState;
    }
    private void OnDestroy()
    {
        Main.OnChangeState -= Main_OnChangeState;
    }

    private IEnumerator Co_PrePlay()
    {
        /*
        var waitForSeconds1 = new WaitForSeconds(1);
        _text.text = "3";
        yield return waitForSeconds1;
        _text.text = "2";
        yield return waitForSeconds1;
        _text.text = "1";
        yield return waitForSeconds1;
        */

        for (var i = 0; i < _texts.Length; i++)
        {
            _text.text = _texts[i];
            for (var time = 0f; time <= 1f; time += Time.deltaTime)
            {
                _rectTransform.localScale = new Vector3(time, time, 1f);
                _text.color = new Color(1, 1, 1, time);
                yield return null;
            }
        }
        _rectTransform.localScale = Vector3.zero;

        Main.Instance.State = State.Play;
    }

    private void Main_OnChangeState(State state)
    {
        if (state == State.PrePlay)
        {
            _animatorText.SetTrigger(Constants.ANIMATOR_Show);
            this.StartCoroutine(ref _coroutinePrePlay, Co_PrePlay());
        }
        else
        {
            _text.text = "";
            this.StopCoroutine(ref _coroutinePrePlay);
        }
    }
}
