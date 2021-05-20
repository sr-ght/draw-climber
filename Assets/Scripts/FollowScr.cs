using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowScr : MonoBehaviour
{
    [SerializeField] private TargetObjectsScr _targetObject;
    [SerializeField, Range(0.01f, 100f)] private float _maxFallBehind = 10f;
    [SerializeField, Range(0.01f, 100f)] private float _speedMove = 3f;
    [SerializeField, Range(0.01f, 100f)] private float _speedRotate = 3f;

    private void FixedUpdate()
    {
        UpdatePositionAndRotation(Time.deltaTime);
    }

    private void UpdatePositionAndRotation(float deltaTime)
    {
        var cameraPositionFinish = _targetObject.transform.position;
        var cameraPositionStart = cameraPositionFinish - Vector3.ClampMagnitude(cameraPositionFinish - transform.position, _maxFallBehind);
        transform.position = Vector3.Lerp(cameraPositionStart, cameraPositionFinish, deltaTime * _speedMove);

        var cameraRotationStart = transform.rotation;
        transform.LookAt(_targetObject.LookAtObject);
        var cameraRotationEnd = transform.rotation;
        transform.rotation = Quaternion.Lerp(cameraRotationStart, cameraRotationEnd, deltaTime * _speedRotate);
    }
}
