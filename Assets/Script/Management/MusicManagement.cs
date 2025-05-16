using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManagement : MonoBehaviour
{
    public static MusicManagement instance;
    public AudioSource source_SFX;
    public AudioSource source_BGM;
    public AudioClip clip_SFX;
    public AudioClip clip_BGM;
    public AudioClip dropZone_SFX;
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

    public void PlayDropZoneSFX()
    {
        //判断 是否关闭音效
        if (PlayerPrefs.GetInt(GameManager.soundSetKey) == 0)
        {
            source_SFX.clip = dropZone_SFX;
            source_SFX.Play();
        }
    }
}
