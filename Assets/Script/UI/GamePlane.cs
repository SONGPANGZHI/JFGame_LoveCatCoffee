using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlane : MonoBehaviour
{
    [SerializeField]
    private Button setting_BTN;         //设置按钮
    [SerializeField]
    private Button clear_BTN;           //清除按钮
    [SerializeField]
    private Button speed_BTN;           //加速按钮
    [SerializeField]
    private Button perspective_BTN;     //透视按钮

    [SerializeField]
    private List<GameObject> clear_Prop;           //清除道具图
    [SerializeField]
    private List<GameObject> speed_Prop;
    [SerializeField]
    private List<GameObject> perspective_Prop;


    [SerializeField]
    private RectTransform buttom_Menu;

    public List<PropData> propData;     //暂定 0 = 清除 ； 1 = 加速 ； 2 = 透视 ； 

    private void Awake()
    {
        setting_BTN.onClick.AddListener(SettingClick);
        clear_BTN.onClick.AddListener(ClearClick);
        speed_BTN.onClick.AddListener(SpeedClick);
        perspective_BTN.onClick.AddListener(PerspectiveClick);
    }

    //游戏界面初始化
    public void GamePlaneInit()
    {
        GamePlaneInitData();
        buttom_Menu.DOMoveY(143, 0.1f); 
    }


    //设置
    private void SettingClick()
    {
        //打开设置界面
        UIManagement.Instance.OpenSettingPlane(true);
    }

    #region 清除道具

    //更新清除道具显示
    public void UpadteClearShow()
    {
        clear_Prop[0].SetActive(false);
        clear_Prop[1].SetActive(true);
    }

    //清除
    private void ClearClick()
    {
        //清除道具使用 先看广告获得
        if (PlayerPrefs.HasKey("ClearPropKey"))
        {
            //道具使用
            GameLevelManagement.Instance.ClearPropUse();
            clear_BTN.interactable = false;
            Debug.Log("道具使用---");
        }
        else
        {
            //看广告获得
            //激励视频添加道具
            Debug.LogError("打开清除道具获得界面");
            UIManagement.Instance.OpenCommonPlane(CommonPlaneType.Prop, propData[0]);
        }
    }

    #endregion

    #region 加速

    //更新加速道具显示
    public void UpdateSpeedPropShow()
    {
        speed_Prop[0].SetActive(false);
        speed_Prop[1].SetActive(true);
    }

    //加速
    private void SpeedClick()
    {
        //清除加速使用
        if (PlayerPrefs.HasKey("SpeedPropKey"))
        {
            //道具使用
            GameLevelManagement.Instance.SpeedPropUse();
            speed_BTN.interactable = false;
            Debug.Log("道具使用---");
        }
        else
        {
            //看广告获得
            //激励视频添加道具
            Debug.Log("打开加速道具界面");
            UIManagement.Instance.OpenCommonPlane(CommonPlaneType.Prop, propData[1]);
        }
    }

    #endregion

    #region 透视

    //更新透视道具显示
    public void UpadtePerspectivePropShow()
    {
        perspective_Prop[0].SetActive(false);
        perspective_Prop[1].SetActive(true);
    }

    //透视
    private void PerspectiveClick()
    {
        //清除透视使用
        if (PlayerPrefs.HasKey("PerspectivePropKey"))
        {
            //道具使用
            Debug.Log("道具使用---");
            perspective_BTN.interactable = false;
        }
        else
        {
            //看广告获得
            //激励视频添加道具
            Debug.Log("打开加速道具界面");
            UIManagement.Instance.OpenCommonPlane(CommonPlaneType.Prop, propData[2]);
        }
    }

    #endregion

    //界面数据初始化
    public void GamePlaneInitData()
    {
        clear_Prop[1].SetActive(false);
        clear_Prop[0].SetActive(true);
        clear_BTN.interactable = true;
        PlayerPrefs.DeleteKey("ClearPropKey");

        speed_Prop[1].SetActive(false);
        speed_Prop[0].SetActive(true);
        speed_BTN.interactable = true;
        PlayerPrefs.DeleteKey("SpeedPropKey");

        perspective_Prop[1].SetActive(false);
        perspective_Prop[0].SetActive(true);
        perspective_BTN.interactable = true;
        PlayerPrefs.DeleteKey("PerspectivePropKey");

        PlayerPrefs.DeleteKey(SettingPlane.propUserKey);
    }


    //关闭
    public void CloseGamePlane()
    {
        buttom_Menu.DOMoveY(-150, 0.5f).OnComplete(() => 
        {
            this.gameObject.SetActive(false);
        });
    }
}
