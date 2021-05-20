using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;

public class TargetObjectsScr : MonoBehaviour
{
    [SerializeField] private PositionConstraint _positionConstraint;
    [SerializeField] private LookAtConstraint _lookAtConstraint;
    [SerializeField] private TargetObject[] _targetObjects;

    public Transform LookAtObject { get; private set; }

    private void Awake()
    {
        Main.OnChangeState += Main_OnChangeState;
    }

    private void OnDestroy()
    {
        Main.OnChangeState -= Main_OnChangeState;
    }

    public void SetTarget(State state)
    {
        var targetObject = _targetObjects.FirstOrDefault(t => t.States.Any(s => s == state));
        if (targetObject != null)
        {
            LookAtObject = targetObject.SetTarget(_positionConstraint, _lookAtConstraint);
        }
    }

    private void Main_OnChangeState(State state)
    {
        SetTarget(state);
    }
}
