using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class AudioManager : MonoSingleton<AudioManager>
{
    [HideInInspector]
    public float volume = 1f;
    Transform audioRoot;
    private Dictionary<string, AudioClip> audioPool;
    private Dictionary<string, AudioClip> audioSourcePool;

    public const string AudioPath = "Audio";

    public void Init()
    {
        audioRoot = transform.Find("Audio");

        audioPool = new Dictionary<string, AudioClip>();
        List<AudioClip> list = Resources.LoadAll<AudioClip>(AudioPath).ToList();
        list.ForEach(audio =>
        {
            audioPool.Add(audio.name, audio);
        });
        audioPool = list.ToDictionary(audio => audio.name);
    }

    /// <summary>
    /// 播放音频
    /// </summary>
    /// <param name="audioName">音频素材名称</param>
    /// <param name="isLoop">是否循环</param>
    /// <param name="isGradualVolume">播放的时候声音是否是逐渐的增加到满</param>
    /// <param name="gradualVolumeTime">逐渐增加音量的总时间</param>
    public void Play(string audioName, bool isLoop = false, bool isGradualVolume = false, float gradualVolumeTime = 5f)
    {
        if (!audioPool.ContainsKey(audioName))
            throw new Exception("the audio is not exist : " + audioName);

        var audio = audioPool[audioName];

        GameObject obj = new GameObject(audioName);
        obj.transform.parent = audioRoot;
        AudioSource source = obj.AddComponent<AudioSource>();
        StartCoroutine(this.PlayClipEndDestroy(audio, obj, isLoop));
        source.pitch = 1f;
        source.clip = audio;
        source.loop = isLoop;

        if (isGradualVolume)
        {
            source.volume = 0;
            StartCoroutine(GradualVolume(audio, obj, gradualVolumeTime > 0, 0, volume, gradualVolumeTime));
        }
        else
            source.volume = volume;

        source.Play();
    }

    /// <summary>
    /// 播放玩音效删除物体
    /// </summary>
    /// <param name="audioclip"></param>
    /// <param name="soundobj"></param>
    /// <returns></returns>
    private IEnumerator PlayClipEndDestroy(AudioClip audioclip, GameObject soundobj, bool isLoop = false)
    {
        if (soundobj == null || audioclip == null)
            yield break;
        else
        {
            if (!isLoop)
            {
                yield return new WaitForSeconds(audioclip.length * Time.timeScale);
                Destroy(soundobj);
            }
        }
    }


    /// <summary>
    /// 停止播放音效 (目前只停止一个同名音效)
    /// </summary>
    /// <param name="audioName"></param>
    /// <param name="isGradualVolume">音量是否逐渐的消失</param>
    public void StopAudio(string audioName, bool isGradualVolume = false)
    {
        if (!audioPool.ContainsKey(audioName))
        {
            Debug.LogWarning("the aduio is not exist : " + audioName);
            return;
        }
        var child = audioRoot.transform.Find(audioName);
        if (child != null)
        {
            if (isGradualVolume)
            {
                StartCoroutine(GradualVolume(audioPool[audioName], child.gameObject, false, volume, 0, 3.0f));
            }
            else
                Destroy(child.gameObject);
        }

    }

    /// <summary>
    /// 停止所有的音效
    /// </summary>
    /// <param name="isGradualVolume">音量是否逐渐的消失</param>
    /// <param name="gradualTime"></param>
    public void StopAllAudio(bool isGradualVolume = false, float gradualTime = 3.0f)
    {
        if (audioRoot != null)
        {
            for (int i = audioRoot.childCount - 1; i >= 0; --i)
            {
                var child = audioRoot.GetChild(i);
                if (child != null)
                {
                    if (isGradualVolume)
                        StartCoroutine(GradualVolume(audioPool[child.name], child.gameObject, false, volume, 0, gradualTime));
                    else
                        Destroy(child.gameObject);
                }
            }
        }
    }

    /// <summary>
    /// 渐变声音
    /// </summary>
    /// <param name="audioclip">clip</param>
    /// <param name="soundobj">正在播放的 AudioSource </param>
    /// <param name="isAdd">增大音量还是减少音量</param>
    /// <param name="initVolume">初始音量</param>
    /// <param name="toVolume">目标音量</param>
    /// <param name="time">总共的 播放/停止 时间</param>
    /// <returns></returns>
    IEnumerator GradualVolume(AudioClip audioclip, GameObject soundobj, bool isAdd, float initVolume, float toVolume, float time)
    {
        float v = initVolume;
        while (true)
        {
            if (soundobj == null || audioclip == null)
                yield break;
            
            var source = soundobj.GetComponent<AudioSource>();
            v += (toVolume - initVolume) / (time == 0 ? 1 : time) * Time.deltaTime;
            source.volume = v;
            if (isAdd)
            {
                if (v >= toVolume)
                {
                    source.volume = toVolume;
                    yield break;
                }
            }
            else
            {
                if (v <= toVolume)
                {
                    source.volume = toVolume;
                    if (soundobj != null)
                        Destroy(soundobj);
                    yield break;
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
