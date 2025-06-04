using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainPlane : MonoBehaviour
{
    [SerializeField]
    private Button startPlay_BTN;                   //开始按钮

    [SerializeField]
    private Button challenge_BTN;                   //挑战按钮

    [SerializeField]
    private Button illustrations_BTN;               //图鉴按钮

    [SerializeField]
    private Button pretend_BTN;                     //装扮按钮

    [SerializeField]
    private Button setting_BTN;                     //设置按钮

    [SerializeField]
    private TMP_Text currentLevel_TMP;           //当前关卡文本

    [SerializeField]
    private TMP_Text challenge_TMP;              //挑战次数文本

    [SerializeField]
    private Transform bottom_OBJ;

    public GameObject dressBG;                      //装扮背景

    public GameObject redPoint;

    private void Awake()
    {
        startPlay_BTN.onClick.AddListener(StartPlayClick);
        challenge_BTN.onClick.AddListener(ChallengeClick);
        illustrations_BTN.onClick.AddListener(IllustrationsClick);
        pretend_BTN.onClick.AddListener(PretendClick);
        setting_BTN.onClick.AddListener(SettingClick);
       
    }


    //界面初始化
    public void InitPlane()
    {
        bottom_OBJ.DOMoveY(300,0.3f);
        currentLevel_TMP.text = "当前关卡：" + (PlayerPrefs.GetInt(GameManager.CurrentGameLevelKey));
        if (PlayerPrefs.HasKey(UIManagement.redPointKey))
            OpenRedPiont();
        else
            CloseRedPonit();

        dressBG.SetActive(true);
        DetermineOpenDress();

        //新手引导
        //Guidance();
    }

    public void Guidance()
    {
        if (GuidancePlane.Instance.JudgeWhetherOpenGuide(0))
        {
            //新手引导点击开始
            Invoke("GuidanceStartPlay", 0.5f);
        }
        else if (!GuidancePlane.Instance.JudgeWhetherOpenGuide(3) && GuidancePlane.Instance.JudgeWhetherOpenGuide(4))
        {
            //新手引导装扮
            Invoke("GuidanceDressUp", 0.5f);
        }
    }

    //新手引导点击开始
    public void GuidanceStartPlay()
    {
        UIManagement.Instance.OpenGuidancePlane();
        GuidancePlane.Instance.GuidanceInit(0, startPlay_BTN.transform.position);
    }

    //新手引导装扮
    public void GuidanceDressUp()
    {
        UIManagement.Instance.OpenGuidancePlane();
        GuidancePlane.Instance.GuidanceInit(4, pretend_BTN.transform.position);
    }

    //开始
    public void StartPlayClick()
    {
        //加载场景
        UIManagement.Instance._isChallengBool = false;
        MusicManagement.instance.ClickPlaySFX();
        UIManagement.Instance.sceneName = "PlayGame";
        GameManager.Instance.SavaChallengTime();
        UIManagement.Instance.OpenLoadingPlane();

    }

    //挑战
    private void ChallengeClick()
    {
        //加载场景
        UIManagement.Instance._isChallengBool = true;
        MusicManagement.instance.ClickPlaySFX();
        UIManagement.Instance.sceneName = "PlayGame";
        UIManagement.Instance.OpenLoadingPlane();
    }

    //图鉴
    private void IllustrationsClick()
    {
        //打开图鉴界面
    }

    public void DetermineOpenDress()
    {
        if (PlayerPrefs.GetInt(GameManager.CurrentGameLevelKey) > 1)
        {
            pretend_BTN.gameObject.SetActive(true);
        }
        else
        {
            pretend_BTN.gameObject.SetActive(false);
        }
    }

    //装扮
    private void PretendClick()
    {
        //打开装扮界面
        UIManagement.Instance._isChallengBool = false;
        MusicManagement.instance.ClickPlaySFX();
        //UIManagement.Instance.OpenFurnitureUpgradePlane();
        UIManagement.Instance.OpenFurnitureUpgradePlane();
        dressBG.SetActive(false);
        ClosePlane();
        UIManagement.Instance.CloseMainPlane();

    }

    //设置界面
    private void SettingClick()
    {
        MusicManagement.instance.ClickPlaySFX();
        UIManagement.Instance.OpenSettingPlane();
    }

    public void ClosePlane()
    {
        bottom_OBJ.DOMoveY(-300, 0.3f);
    }

    //打开红点
    public void OpenRedPiont()
    {
        redPoint.SetActive(true);
    }

    public void CloseRedPonit()
    {
        redPoint.SetActive(false);
    }

}
