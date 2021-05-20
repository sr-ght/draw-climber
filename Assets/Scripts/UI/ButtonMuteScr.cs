using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMuteScr : ButtonToggleScr
{
    [SerializeField] private ButtonToggleScr _buttonSettings;
    [SerializeField] protected List<Animator> _animatorsShow;
    [SerializeField] protected Animator _animatorMute;

    public override bool State
    {
        get => base.State;
        set
        {
            base.State = value;
            _animatorMute.SetBool(Constants.ANIMATOR_Show, _state);
        }
    }

    protected void Awake() =>
        _buttonSettings.OnChangeButtonState += ButtonSettings_OnChangeButtonState;

    protected void OnDestroy() =>
        _buttonSettings.OnChangeButtonState -= ButtonSettings_OnChangeButtonState;

    protected void ButtonSettings_OnChangeButtonState(bool stateButtonSettings)
    {
        if (_animatorsShow != null)
        {
            _animatorsShow.ForEach(a => a.SetBool(Constants.ANIMATOR_Show, stateButtonSettings));
        }
        _animatorMute.SetBool(Constants.ANIMATOR_Show, stateButtonSettings && _state);
    }
}
