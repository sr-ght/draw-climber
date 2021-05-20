using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class CameraScr : MonoBehaviour
{
    public static CameraScr Instance { get; private set; }

    [SerializeField] private Camera _camera;

    public Camera Camera => _camera;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
    }

}
