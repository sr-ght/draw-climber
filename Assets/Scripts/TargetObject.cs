using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

[Serializable]
public class TargetObject
{
    [SerializeField] private State[] _states;
    [SerializeField] private Transform _targetPosition;
    [SerializeField] private Transform _targetLookAt;
    [SerializeField] private Vector3 _offsetPosition;

    public State[] States { get => _states; }
    public Transform TargetPosition { get => _targetPosition; }
    public Transform TargetLookAt { get => _targetLookAt; }
    public Vector3 OffsetPosition { get => _offsetPosition; }

    public Transform SetTarget(PositionConstraint positionConstraint, LookAtConstraint lookAtConstraint)
    {
        var sourcePosition = new ConstraintSource() { sourceTransform = _targetPosition, weight = 1f };
        positionConstraint.SetSource(0, sourcePosition);
        positionConstraint.translationOffset = _offsetPosition;

        var sourceLookAt = new ConstraintSource() { sourceTransform = _targetLookAt, weight = 1f };
        lookAtConstraint.SetSource(0, sourceLookAt);

        return TargetLookAt;
    }
}
