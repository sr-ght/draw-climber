using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class ClimberScr : MonoBehaviour
{
    [Header("Climber")]
    [SerializeField] private Rigidbody2D _climberRigitBody2D;
    [SerializeField] private ConstantForce2D _climberConstantForce2D;
    [SerializeField] private Vector2 _helpForce = new Vector2(3, -3);

    [Header("Shoulders")]
    [SerializeField] private Transform _shoulders;
    [SerializeField] private CollisionDetectorScr _shouldersCollisionDetector;
    [SerializeField] private Rigidbody2D _shouldersRB;
    [SerializeField] private float _angularVelocity;

    [Header("Arms")]
    [SerializeField, Range(1f, 5f)] private float _maxArmLength;
    [SerializeField, Range(0.0001f, 0.1f)] private float _lineTolerance = 0.01f;
    [SerializeField] private Arm[] _arms;
    [SerializeField] private GameObject _prefabBone;
    [SerializeField] private GameObject _prefabJoint;

    private readonly WaitForFixedUpdate _waitForFixedUpdate = new WaitForFixedUpdate();
    private List<Vector2> _armPoints = new List<Vector2>() { Vector2.zero, Vector2.one * 0.5f, Vector2.right };

    private Coroutine _coroutineRun;
    private Coroutine _coroutineRespawnNewArms;
    private Coroutine _coroutineLevitation;
    private Action _respawnNewArmsAction;

    public Vector3 StartPosition { get; set; }
    public bool IsRun
    {
        get => _coroutineRun != null;
        set
        {
            if (IsRun != value)
            {
                if (value)
                {
                    this.StartCoroutine(ref _coroutineRun, Co_Run());
                }
                else
                {
                    _shouldersRB.angularVelocity = 0f;
                    _climberConstantForce2D.force = Vector2.zero;
                    this.StopCoroutine(ref _coroutineRun);
                }
            }
        }
    }

    private bool _isRotationStuck;
    private bool IsRotationStuck
    {
        get => _isRotationStuck;
        set
        {
            if (_isRotationStuck != value)
            {
                _isRotationStuck = value;
                if (_isRotationStuck)
                {
                    this.StopCoroutine(ref _coroutineRespawnNewArms);
                    _shouldersCollisionDetector.OnCollisionStay += CollisionDetector_OnCollisionStay;
                }
                else
                {
                    _shouldersCollisionDetector.OnCollisionStay -= CollisionDetector_OnCollisionStay;
                }
            }
        }
    }

    private void OnEnable()
    {
        Main.OnChangeState += Main_OnChangeState;
        DrawArmScr.OnChangeArmPoints += DrawArmScr_OnChangeArmPoints;
    }

    private void OnDisable()
    {
        Main.OnChangeState -= Main_OnChangeState;
        DrawArmScr.OnChangeArmPoints -= DrawArmScr_OnChangeArmPoints;
    }

    private IEnumerator Co_RespawnNewArms()
    {
        DestroyArms();
        var countArmPoints = _armPoints.Count;
        var boneLocalScale = _prefabBone.transform.localScale;
        for (var i = 0; i < countArmPoints - 1; i++)
        {
            var from = _armPoints[i];
            var to = _armPoints[i + 1];

            var boneLocalPosition = (from + to) * 0.5f;
            var boneLocalDirection = to - from;
            boneLocalScale.x = boneLocalDirection.magnitude;
            var boneLocalRotation = Quaternion.Euler(0f, 0f, boneLocalDirection.Atan2() * Mathf.Rad2Deg);

            // Add bone
            foreach (var arm in _arms)
            {
                GameObject bone;
                var boneInd = i * 2;
                if (boneInd < arm.Bones.Count)
                {
                    bone = arm.Bones[boneInd];
                    bone.SetActive(true);
                }
                else
                {
                    bone = Instantiate(_prefabBone, arm.Transform);
                    arm.Bones.Add(bone);
                }
                bone.transform.SetLocalPositionAndRotation(boneLocalPosition, boneLocalRotation);
                bone.transform.localScale = boneLocalScale;
            }

            // Add "joint" (bone)
            foreach (var arm in _arms)
            {
                GameObject bone;
                var boneInd = i * 2 + 1;
                if (boneInd < arm.Bones.Count)
                {
                    bone = arm.Bones[boneInd];
                    bone.SetActive(false);
                }
                else
                {
                    bone = Instantiate(_prefabJoint, arm.Transform);
                    arm.Bones.Add(bone);
                }
                bone.transform.SetPositionAndRotation(arm.Transform.localToWorldMatrix.MultiplyPoint(_armPoints[i + 1]), Quaternion.identity);
            }
        }

        var bonesCount = (countArmPoints - 1) * 2;
        for (var i = 0; i < bonesCount; i++)
        {
            foreach (var arm in _arms)
            {
                arm.Bones[i].SetActive(false);
            }
        }

        for (var i = 0; i < bonesCount; i++)
        {
            foreach(var arm in _arms)
            {
                arm.Bones[i].SetActive(true);
            }
            yield return _waitForFixedUpdate;
        }
    }

    private IEnumerator Co_Run()
    {
        _shoulders.localRotation = Quaternion.identity;
        var lastAngle = float.MaxValue;
        var deltaAngel = Mathf.Abs(_angularVelocity) * Time.fixedDeltaTime * Mathf.Deg2Rad;
        while (true)
        {
            _shouldersRB.angularVelocity = _angularVelocity;
            var angle = ((Vector2)_shoulders.right).Atan2();
            var difference = Mathf.Abs(lastAngle - angle);
            if (difference <= deltaAngel * 0.01f)
            {
                IsRotationStuck = true;
            }
            else
            {
                _climberConstantForce2D.force = Vector2.Lerp(Vector2.zero, _helpForce, difference / deltaAngel);
                IsRotationStuck = false;
            }
            lastAngle = angle;
            yield return _waitForFixedUpdate;
        }
    }

    private IEnumerator Co_Levitation()
    {
        var originalPosition = StartPosition;
        var pi2 = Mathf.PI * 2f;
        while (true)
        {
            transform.position = originalPosition + Vector3.up * Mathf.Sin(Time.time * pi2) * 0.1f;
            yield return null;
        }
    }

    private void DestroyArms()
    {
        foreach (var bones in _arms.Select(arm => arm.Bones))
        {
            bones.ForEach(bone => bone.SetActive(false));
        }
    }

    private void RespawnNewArms()
    {
        _shoulders.rotation = Quaternion.identity;
        this.StartCoroutine(ref _coroutineRespawnNewArms, Co_RespawnNewArms());
    }

    private void СutArms(GameObject bone)
    {
        foreach (var arm in _arms)
        {
            var lastBone = arm.CutArm(bone);
            if (lastBone)
            {
                break;
            }
        }
    }

    private void SetLevitation(bool levitation)
    {
        _climberRigitBody2D.isKinematic = levitation;
        if (levitation)
        {
            _shouldersRB.velocity = _climberRigitBody2D.velocity = Vector2.zero;
            _shouldersRB.angularVelocity = 0f;
            _shouldersRB.isKinematic = true;
            _shoulders.rotation = Quaternion.identity;
            this.StartCoroutine(ref _coroutineLevitation, Co_Levitation());
        }
        else
        {
            _shouldersRB.isKinematic = false;
            this.StopCoroutine(ref _coroutineLevitation);
        }
    }

    private void CollisionDetector_OnCollisionStay(object collision)
    {
        var mayBeBone = (collision as Collision2D).contacts[0].otherCollider.gameObject;
        СutArms(mayBeBone);
    }

    private void DrawArmScr_OnChangeArmPoints(List<Vector2> points)
    {
        LineUtility.Simplify(points, 0.01f, points);
        _armPoints = points.Select(p => p * _maxArmLength).ToList();
        _respawnNewArmsAction?.Invoke();
    }

    private void Main_OnChangeState(State state)
    {
        SetLevitation(!(state == State.Play || state == State.Finish));
        IsRun = state == State.Play;

        if (state == State.Menu || state == State.Finish || state == State.NextLevel)
        {
            DestroyArms();
            _respawnNewArmsAction = null;
        }
        else if (state == State.Start)
        {
            DestroyArms();
            _respawnNewArmsAction = () => { RespawnNewArms(); Main.Instance.State = State.PrePlay; };
        }
        else if (state == State.Retry)
        {
            DestroyArms();
            _respawnNewArmsAction = () => { RespawnNewArms(); Main.Instance.State = State.Play; };
        }
        else
        {
            _respawnNewArmsAction = () => { RespawnNewArms(); };
        }

        if (state == State.Retry)
        {
            TranslateToStartPosition();
        }
    }

    public void TranslateToStartPosition() =>
        transform.position = StartPosition;
}
