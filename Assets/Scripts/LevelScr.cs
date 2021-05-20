using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScr : MonoBehaviour
{
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Transform _finishPoint;
    [SerializeField] private Transform _fallDownPoint;

    //private ClimberScr _climber;

    public Vector3 StartPos { get => _startPoint.transform.position; }
    public Vector3 FinishPos { get => _finishPoint.transform.position; }
    public float FallDownPosY { get => _fallDownPoint.transform.position.y; }

    public void Activate(ClimberScr climber)
    {
        gameObject.SetActive(true);
        climber.StartPosition = _startPoint.position;
        climber.TranslateToStartPosition();
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
