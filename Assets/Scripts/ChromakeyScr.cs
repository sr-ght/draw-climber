using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChromakeyScr : MonoBehaviour
{
    [SerializeField] private Material _material;

    public float _dChroma;

    public void Update()
    {
        _material.SetFloat(Constants.SHADER_DChroma, _dChroma);
    }
}
