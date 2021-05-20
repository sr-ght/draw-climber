using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[Serializable]
public class VideoChromaKey
{
    [SerializeField] private VideoClip _videoClip;
    [SerializeField] private Color _keyColor = Color.green;
    [SerializeField, Range(0f, 1f)] private float _dChroma = 0.25f;
    [SerializeField, Range(0f, 1f)] private float _dChromaTolerance = 0.01f;

    public VideoClip VideoClip { get => _videoClip; }
    public Color KeyColor { get => _keyColor; }
    public float DChroma { get => _dChroma; }
    public float DChromaTolerance { get => _dChromaTolerance; }
}
