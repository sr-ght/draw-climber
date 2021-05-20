using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoPlayerManagerScr : MonoBehaviour
{
    [SerializeField] private List<VideoPlayerScr> _videoPlayers;
    [SerializeField] private List<VideoChromaKey> _videos;
    [SerializeField] private List<GameStateVideoList> _statesVideos;
    [SerializeField] private List<DrawStateVideoList> _drawsVideos;

    private int _currentPlayer = 0;

    private void Awake()
    {
        Main.OnChangeState += Main_OnChangeState;
        DrawArmScr.OnChangeDrawState += DrawArmScr_OnChangeDrawState;
    }
    private void OnDestroy()
    {
        Main.OnChangeState -= Main_OnChangeState;
        DrawArmScr.OnChangeDrawState -= DrawArmScr_OnChangeDrawState;
    }

    private void Main_OnChangeState(State state)
    {
        var videoList = _statesVideos.FirstOrDefault(vl => vl.State == state);
        PlayVideoFromList(videoList);
    }

    private void DrawArmScr_OnChangeDrawState(bool isStartDraw)
    {
        var videoList = _drawsVideos.FirstOrDefault(vl => vl.IsStartDraw == isStartDraw);
        PlayVideoFromList(videoList);
    }

    private void PlayVideoFromList(VideoList videoList)
    {
        if (videoList == null)
            return;

        var video = videoList.Videos[UnityEngine.Random.Range(0, videoList.Videos.Count)];
        var videoChromaKey = _videos.Find(v => v.VideoClip == video);
        if (videoChromaKey == null)
            return;

        var currentPlayer = _videoPlayers[_currentPlayer];
        currentPlayer.PlayVideoDelay(videoChromaKey, videoList.TimeDelay);

        for (int i = 0; i < _videoPlayers.Count; i++)
        {
            var locPos = _videoPlayers[i].RectTransform.localPosition;
            _videoPlayers[i].RectTransform.localPosition = new Vector3(locPos.x, locPos.y, _videoPlayers.Count - i);
        }
        _currentPlayer = (_currentPlayer + 1) % _videoPlayers.Count;
    }
}
