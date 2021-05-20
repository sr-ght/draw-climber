using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ButtonSetStateScr : MonoBehaviour
{
    [SerializeField] private State _setState;
    [SerializeField] private State[] _statesWhenActive;
    [SerializeField] private List<Animator> _animators;

    private Action _onClickAction;

    private void Awake() =>
        Main.OnChangeState += Main_OnChangeState;

    private void OnDestroy() =>
        Main.OnChangeState -= Main_OnChangeState;

    public void OnClick() =>
        _onClickAction?.Invoke();

    private void Main_OnChangeState(State state)
    {
        if (_statesWhenActive.Any(s => s == state))
        {
            _onClickAction = () => Main.Instance.State = _setState;
            _animators.ForEach(a => a?.SetBool(Constants.ANIMATOR_Show, true));
        }
        else
        {
            _onClickAction = null;
            _animators.ForEach(a => a?.SetBool(Constants.ANIMATOR_Show, false));
        }
    }
}
