using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class SettingPlane : MonoBehaviour
{
    [SerializeField]
    private Toggle music_Toggle;
    [SerializeField]
    private Toggle sound_Toggle;
    [SerializeField]
    private Toggle shake_Toggle;

    [SerializeField]
    private Button giveUP_BTN;
    [SerializeField]
    private Button back_BTN;

    [SerializeField]
    private GameObject bottom_Menu_Main;
    [SerializeField]
    private GameObject bottom_Menu_Game;

    //保存设置Key 不需要修改
    private const string musicSetKey = "MusicSettingKey";
    private const string soundSetKey = "SoundSettingKey";
    private const string shakeSetKey = "ShakeSettingKey";
    public const string propUserKey = "PropUserKey";
    private void Awake()
    {
        giveUP_BTN.onClick.AddListener(GiveUpChallengeClick);
        back_BTN.onClick.AddListener(ClosePlane);
        music_Toggle.onValueChanged.AddListener(MusicClick);
        sound_Toggle.onValueChanged.AddListener(SoundClick);
        shake_Toggle.onValueChanged.AddListener(ShakeClick);
    }

    /// <summary>
    /// 初始化界面  _isGame = true 在游戏界面打开设置  _isGame = false 在主界面打开设置
    /// </summary>
    /// <param name="_isGame"></param>
    public void InitSetPlane(bool _isGame = false)
    {
        transform.GetChild(0).DOScale(new Vector3(1,1,1),0.3f);
        bottom_Menu_Game.SetActive(false);
        bottom_Menu_Main.SetActive(false);

        if (_isGame)
            bottom_Menu_Game.SetActive(true);
        else
            bottom_Menu_Main.SetActive(true);

        //初始化数据
        InitSetData();
    }
    
    //初始化姐界面数据
    public void InitSetData()
    {
        music_Toggle.isOn = FindSettingSave(musicSetKey);
        sound_Toggle.isOn = FindSettingSave(soundSetKey);
        shake_Toggle.isOn = FindSettingSave(shakeSetKey);
    }

    //放弃挑战
    private void GiveUpChallengeClick()
    {
        ClosePlane();
        //需要判断是否 使用过道具
        if (PlayerPrefs.HasKey(propUserKey))
        {
            //打开结束界面
            UIManagement.Instance.OpenGameOverPlane();
        }
        else
        {
            //打开复活界面
            UIManagement.Instance.OpenCommonPlane(CommonPlaneType.Affirm);
        }
    }

    public void ClosePlane()
    {
        transform.GetChild(0).DOScale(new Vector3(0, 0, 0), 0.3f).OnComplete(() => 
        {
            this.gameObject.SetActive(false);
        });
          
    }

    //音乐   0 为 打开音乐，1 关闭音乐
    private void MusicClick(bool open)
    {
        music_Toggle.isOn = open;
        if (open)
        {
            //打开音乐
            PlayerPrefs.SetInt(musicSetKey, 0);
        }
        else 
        {
            //关闭
            PlayerPrefs.SetInt(musicSetKey, 1);
        }
    }

    //音效
    private void SoundClick(bool open)
    {
        sound_Toggle.isOn = open;
        if (open)
        {
            //打开音效
            PlayerPrefs.SetInt(soundSetKey, 0);
        }
        else
        {
            //关闭
            PlayerPrefs.SetInt(soundSetKey, 1);
        }
    }

    //震动
    private void ShakeClick(bool open)
    {
        shake_Toggle.isOn = open;
        if (open)
        {
            //打开震动
            PlayerPrefs.SetInt(shakeSetKey, 0);
        }
        else
        {
            //关闭
            PlayerPrefs.SetInt(shakeSetKey, 1);
        }
    }


    //根据 Key 返回当前保存的Bool; 没有找到Key 默认打开
    public bool FindSettingSave(string setKey)
    {
        if(PlayerPrefs.GetInt(setKey) == 0 || !PlayerPrefs.HasKey(setKey))
            return true;
        else
            return false;

    }
}
