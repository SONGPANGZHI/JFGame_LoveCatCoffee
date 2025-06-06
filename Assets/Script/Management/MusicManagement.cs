using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManagement : MonoBehaviour
{
    public static MusicManagement instance;
    public AudioSource source_SFX;
    public AudioSource source_BGM;
    public AudioSource special_SFX;
    public AudioClip clip_SFX;
    public AudioClip clip_BGM;
    public AudioClip dropZone_SFX;
    public AudioClip destruction_SFX;
    private void Awake()
    {
        if(instance==null)
            instance = this;
    }

    private void Start()
    {
        PlayBGM();
    }

    //点击播放音效
    public void ClickPlaySFX()
    {
        //判断 是否关闭音效
        if (PlayerPrefs.GetInt(GameManager.soundSetKey) == 0)
        {
            source_SFX.clip = clip_SFX;
            source_SFX.Play();
        }
    }


    //播放BGM
    public void PlayBGM()
    {
        if (PlayerPrefs.GetInt(GameManager.musicSetKey) == 0)
        {
            source_BGM.clip = clip_BGM;
            source_BGM.Play();
        }
    }

    public void CloseBGM()
    {
        source_BGM.clip = clip_BGM;
        source_BGM.Stop();
    }

    //放置音效
    public void PlayDropZoneSFX()
    {
        //判断 是否关闭音效
        if (PlayerPrefs.GetInt(GameManager.soundSetKey) == 0)
        {
            source_SFX.clip = dropZone_SFX;
            source_SFX.Play();
        }
    }

    //播放销毁音效
    public void PlayDestorySFX()
    {
        //判断 是否关闭音效
        if (PlayerPrefs.GetInt(GameManager.soundSetKey) == 0)
        {
            special_SFX.clip = destruction_SFX;
            special_SFX.Play();
        }
    }
}
