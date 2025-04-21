using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManagement : MonoBehaviour
{
    public static MusicManagement instance;
    public AudioSource source_SFX;
    public AudioClip clip_SFX;
    private void Awake()
    {
        if(instance==null)
            instance = this;
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

}
