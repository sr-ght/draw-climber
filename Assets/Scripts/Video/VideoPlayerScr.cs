using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoPlayerScr : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private VideoPlayer _videoPlayer;
    [SerializeField] private RawImage _rawImage;

    public Coroutine _coroutinePlayVideo;

    public RectTransform RectTransform { get => _rectTransform; }
    public VideoPlayer VideoPlayer { get => _videoPlayer; }
    public RawImage RawImage { get => _rawImage; }

    private void Awake()
    {
        TimeScr.OnChangeTimeScale += TimeScr_OnChangeTimeScale;
        VideoPlayer.started += Started;
        VideoPlayer.loopPointReached += EndReached;
        //RawImage.material = Instantiate(RawImage.material);
        //RawImage.enabled = false;
    }

    private void OnDestroy()
    {
        TimeScr.OnChangeTimeScale -= TimeScr_OnChangeTimeScale;
        VideoPlayer.started -= Started;
        VideoPlayer.loopPointReached -= EndReached;
    }

    private void TimeScr_OnChangeTimeScale(float scale)
    {
        VideoPlayer.playbackSpeed = scale;
    }

    private void Started(VideoPlayer vp)
    {
        //_rawImage.enabled = true;
    }

    private void EndReached(VideoPlayer vp)
    {
        //_rawImage.enabled = false;
        vp.frame = 0;
    }

    public void PlayVideoDelay(VideoChromaKey videoChromaKey, float delay)
    {
        this.StartCoroutine(ref _coroutinePlayVideo, Co_PlayVideoDelay(videoChromaKey, delay));
    }

    public IEnumerator Co_PlayVideoDelay(VideoChromaKey videoChromaKey, float delay)
    {
        VideoPlayer.Stop();
        VideoPlayer.clip = videoChromaKey.VideoClip;
        //RawImage.material.SetColor(Constants.SHADER_KeyColor, videoChromaKey.KeyColor);
        //RawImage.material.SetFloat(Constants.SHADER_DChroma, videoChromaKey.DChroma);
        //RawImage.material.SetFloat(Constants.SHADER_DChromaT, videoChromaKey.DChromaTolerance);
        VideoPlayer.Prepare();
        if (delay > 0f)
        {
            yield return new WaitForSeconds(delay);
        }
        yield return new WaitUntil(() => VideoPlayer.isPrepared);
        VideoPlayer.Play();
    }
}
