using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using UnityEngine.EventSystems;
using System;
using System.Linq;

public class DrawArmScr : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public static event Action<bool> OnChangeDrawState;
    public static event Action<List<Vector2>> OnChangeArmPoints;

    [SerializeField, Range(0.1f, 10f)] private float _ratio;
    [SerializeField, Range(30, 300)] private int _maxPoints;
    [SerializeField, Range(0f, 1f)] private float _minBoneLength = 0.05f;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private UILineRenderer _lineRenderer;
    [SerializeField] private GameObject _pointBegin;
    [SerializeField] private GameObject _pointEnd;
    [SerializeField] private Animator _animator;

    private Coroutine _coroutineDraw;
    private List<Vector2> _drawPoints = new List<Vector2>();
    private Action _pointerDownAction;
    private Action _pointerUpAction;

    private void Awake()
    {
        Main.OnChangeState += Main_OnChangeState;
    }

    private void OnDestroy()
    {
        Main.OnChangeState -= Main_OnChangeState;
    }

    private void Start()
    {
        DropArms();
    }

    private void DropArms()
    {
        _lineRenderer.Points = new Vector2[] { };
        _pointBegin.SetActive(false);
        _pointEnd.SetActive(false);
    }

    private IEnumerator Co_Draw()
    {
        var minBoneLengthScr = _minBoneLength * _minBoneLength;
        _pointBegin.SetActive(true);
        _pointEnd.SetActive(true);
        while (true)
        {
            var mousePosition = Input.mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, mousePosition, _canvas.worldCamera, out Vector2 localpoint);
            var normalizedPoint = Rect.PointToNormalized(_rectTransform.rect, localpoint);
            if (_drawPoints.Count != 0)
            {
                if (Vector2.SqrMagnitude(_drawPoints.Last() - normalizedPoint) > minBoneLengthScr)
                {
                    _drawPoints.Add(normalizedPoint);
                    _pointEnd.transform.localPosition = localpoint;
                }
            }
            else
            {
                _pointEnd.transform.position = _pointBegin.transform.position = mousePosition;
                _drawPoints.Add(normalizedPoint);
            }
            _lineRenderer.Points = _drawPoints.Select(point => Rect.NormalizedToPoint(_rectTransform.rect, point)).ToArray();

            if (_drawPoints.Count >= _maxPoints)
            {
                FinishDraw();
            }

            yield return null;
        }
    }

    private void StartDraw()
    {
        _lineRenderer.gameObject.SetActive(true);
        _pointBegin.SetActive(false);
        _pointEnd.SetActive(false);
        _drawPoints.Clear();
        this.StartCoroutine(ref _coroutineDraw, Co_Draw());
        OnChangeDrawState?.Invoke(true);
    }

    private void FinishDraw()
    {
        _lineRenderer.gameObject.SetActive(false);
        _pointBegin.SetActive(false);
        _pointEnd.SetActive(false);
        this.StopCoroutine(ref _coroutineDraw);
        var center = _drawPoints[0];
        var scale = new Vector2(_ratio, 1f).normalized;
        OnChangeDrawState?.Invoke(false);
        OnChangeArmPoints?.Invoke(_drawPoints.Select(p => Vector2.Scale(p - center, scale)).ToList());
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _pointerDownAction?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _pointerUpAction?.Invoke();
    }

    private void Main_OnChangeState(State state)
    {
        if (state == State.Menu || state == State.Finish || state == State.NextLevel)
        {
            _pointerDownAction = null;
            _pointerUpAction = FinishDraw;
            _animator.SetBool(Constants.ANIMATOR_Show, false);
        }
        else
        {
            _pointerDownAction = StartDraw;
            _pointerUpAction = FinishDraw;
            _animator.SetBool(Constants.ANIMATOR_Show, true);
        }
    }
}
