using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[Serializable]
public class VideoList
{
    [SerializeField] private float _timeDelay;
    [SerializeField] private List<VideoClip> videos;

    public float TimeDelay { get => _timeDelay; }
    public List<VideoClip> Videos { get => videos; set => videos = value; }
}