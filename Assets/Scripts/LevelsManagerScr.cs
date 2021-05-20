using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class LevelsManagerScr : MonoBehaviour
{
    private static event Action<float> OnChangeClimperPosX;

    [SerializeField] private int _currentLevelInd;
    [SerializeField] private LevelScr[] _levels;
    [SerializeField] private ClimberScr _climber;

    private Coroutine _coroutineCheckFinish;

    private void Awake()
    {
        Main.OnChangeState += Main_OnChangeState;
    }

    private void Main_OnChangeState(State state)
    {
        if (state == State.Play)
        {
            this.StartCoroutine(ref _coroutineCheckFinish, Co_CheckFinish());
        }
        else
        {
            this.StopCoroutine(ref _coroutineCheckFinish);
        }

        if (state == State.Menu)
        {
            InitializeLevel();
        }
        else if (state == State.NextLevel)
        {
            _currentLevelInd = (_currentLevelInd + 1) % _levels.Length;
        }
    }

    private IEnumerator Co_CheckFinish()
    {
        var level = _levels[_currentLevelInd];
        var startPos = level.StartPos;
        var finishPos = level.FinishPos;
        var startPosX = startPos.x;
        var finishPosX = finishPos.x;
        var fallDownY = level.FallDownPosY;
        var waitForFixedUpdate = new WaitForFixedUpdate();
        while (true)
        {
            var climperPos = _climber.transform.position;
            var climberLocalPosX = Mathf.InverseLerp(startPosX, finishPosX, climperPos.x);
            OnChangeClimperPosX?.Invoke(climberLocalPosX);

            if (climperPos.x > finishPosX && climperPos.y > finishPos.y)
            {
                Main.Instance.State = State.Finish;
            }

            if (climperPos.y < fallDownY)
            {
                Main.Instance.State = State.Retry;
            }

            yield return waitForFixedUpdate;
        }
    }

    private void InitializeLevel()
    {
        foreach (var level in _levels)
        {
            level.Deactivate();
        }
        _currentLevelInd %= _levels.Length;
        _levels[_currentLevelInd].Activate(_climber);
    }
}
