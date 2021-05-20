using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    public static readonly int ANIMATOR_Show = Animator.StringToHash("Show");
    public static readonly int SHADER_KeyColor = Shader.PropertyToID("KeyColor");
    public static readonly int SHADER_DChroma = Shader.PropertyToID("_DChroma");
    public static readonly int SHADER_DChromaT = Shader.PropertyToID("_DChromaT");
}
