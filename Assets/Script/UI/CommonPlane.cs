using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CommonPlane : MonoBehaviour
{
    [SerializeField]
    private TMP_Text title_TMP;      //标题 文本
    [SerializeField]
    private TMP_Text desc_TMP;       //介绍 文本

    [SerializeField]
    private Image icon;                 //Icon

    [SerializeField]
    private Button affirm_Abandon_BTN;  //确定界面放弃
    [SerializeField]
    private Button abandon_BTN;         //放弃按钮
    [SerializeField]
    private Button shareGet_BTN;        //分享
    [SerializeField]
    private Button ADGet_BTN;           //看广告
    [SerializeField]
    private Button continue_BTN;        //继续
    [SerializeField]
    private Button back_BTN;            //返回
    [SerializeField]
    private Sprite heaet_IMG;

    public PropData propData;


    private const string resurgence_Title_TMP = "复活";
    private const string resurgence_Desc_TMP = "观看视频可获得一次复活机会";

    private const string affirm_Title_TMP = "确认";
    private const string use_Title_TMP = "使用";
    private void Awake()
    {
        abandon_BTN.onClick.AddListener(AbandonClick);
        shareGet_BTN.onClick.AddListener(ShareGetClick);
        ADGet_BTN.onClick.AddListener(ADGetClick);
        continue_BTN.onClick.AddListener(ContinueClick);
        back_BTN.onClick.AddListener(BackClick);
        affirm_Abandon_BTN.onClick.AddListener(AffirmAbandonClick);
    }


    //界面初始化  复活界面
    public void ResurgenceInitPlane()
    {
        CloseMenuBTN();
        icon.sprite = heaet_IMG;
        icon.SetNativeSize();
        title_TMP.text = resurgence_Title_TMP;
        desc_TMP.text = resurgence_Desc_TMP;
        abandon_BTN.gameObject.SetActive(true);
        shareGet_BTN.gameObject.SetActive(true);
    }

    //初始化 确认界面
    public void AffirmInitPlane()
    {
        CloseMenuBTN();
        icon.sprite = heaet_IMG;
        icon.SetNativeSize();
        title_TMP.text = affirm_Title_TMP;
        desc_TMP.text = resurgence_Desc_TMP;
        affirm_Abandon_BTN.gameObject.SetActive(true);
        continue_BTN.gameObject.SetActive(true);
    }

    //初始化 道具使用界面
    public void PropInitPlane(PropData _propData)
    {
        //传入  道具类型
        CloseMenuBTN();
        propData = _propData;
        icon.sprite = propData.propIcon;
        icon.SetNativeSize();
        desc_TMP.text = propData.propDesc;
        title_TMP.text = use_Title_TMP;
        abandon_BTN.gameObject.SetActive(true);
        ADGet_BTN.gameObject.SetActive(true);
    }

    public void AffirmAbandonClick()
    {
        transform.GetChild(0).DOScale(new Vector3(0, 0, 0), 0.3f).OnComplete(() =>
        {
            ResurgenceInitPlane();
        });
        
    }

    //放弃挑战 打开失败界面
    private void AbandonClick()
    {
        //打开主界面.
        ClosePlane();
        UIManagement.Instance.OpenGameOverPlane();
        //UIManagement.Instance.loadingPlane.gameObject.SetActive(true);
        //UIManagement.Instance.CloseGamePlane();
        //UIManagement.Instance.loadingPlane.LoadUIScene();
    }

    //分享 复活
    private void ShareGetClick()
    {
        //通过分享复活  相当于清除道具
        ClosePlane();
        PlayerPrefs.SetInt(SettingPlane.propUserKey, 1);
        Debug.LogError("分享复活直接使用清除道具----");
    }

    //广告 获得道具
    private void ADGetClick()
    {
        //观看 广告获得道具  点击就可以获得
        ClosePlane();
        IssueReward(propData.awardvideoType);
    }

    //继续游戏
    private void ContinueClick()
    {
        BackClick();
    }

    //后续 广告播放成功 发放奖励
    public void IssueReward(AwardvideoType _awardvideoType)
    {
        switch (_awardvideoType)
        {
            case AwardvideoType.Clear:
                PlayerPrefs.SetString("ClearPropKey", "ClearProp");
                UIManagement.Instance.gamePlane.UpadteClearShow();
                break;
            case AwardvideoType.Speed:
                PlayerPrefs.SetString("SpeedPropKey", "SpeedProp");
                break;
            case AwardvideoType.Perspective:
                PlayerPrefs.SetString("PerspectivePropKey", "PerspectiveProp");
                break;
            case AwardvideoType.Heart:
                break;
            default:
                break;
        }
    }


    //关闭界面
    private void BackClick()
    {
        transform.GetChild(0).DOScale(new Vector3(0, 0, 0), 0.3f).OnComplete(() => 
        {
            this.gameObject.SetActive(false);
            GameManager.Instance.pauseGame = true;
        });
    }
    
    //关闭界面
    public void ClosePlane()
    {
        transform.GetChild(0).DOScale(new Vector3(0, 0, 0), 0.3f).OnComplete(() =>
        {
            this.gameObject.SetActive(false);
        });
    }

    //关闭菜单按钮显示
    public void CloseMenuBTN()
    {
        abandon_BTN.gameObject.SetActive(false);
        shareGet_BTN.gameObject.SetActive(false);
        ADGet_BTN.gameObject.SetActive(false);
        continue_BTN.gameObject.SetActive(false);
        affirm_Abandon_BTN.gameObject.SetActive(false);

        transform.GetChild(0).DOScale(new Vector3(1, 1, 1), 0.3f);
    }
}




