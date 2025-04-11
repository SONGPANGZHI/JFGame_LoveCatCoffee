using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
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

    private void Awake()
    {
        startPlay_BTN.onClick.AddListener(StartPlayClick);
        challenge_BTN.onClick.AddListener(ChallengeClick);
        illustrations_BTN.onClick.AddListener(IllustrationsClick);
        pretend_BTN.onClick.AddListener(PretendClick);
        setting_BTN.onClick.AddListener(SettingClick);

        if (!PlayerPrefs.HasKey("CurrentLevelKey"))
            PlayerPrefs.SetInt("CurrentLevelKey",1);
    }


    //界面初始化
    public void InitPlane()
    {
        bottom_OBJ.DOMoveY(225,0.3f);
        currentLevel_TMP.text = "当前关卡：" + PlayerPrefs.GetInt("CurrentLevelKey");
    }

    //开始
    private void StartPlayClick()
    {
        //加载场景
        UIManagement.Instance.sceneName = "GameLevel";
        GameManager.Instance.NumberLevelChallenges += 1;
        PlayerPrefs.SetInt(GameManager.NumberLevelChallengesKey, GameManager.Instance.NumberLevelChallenges);
        UIManagement.Instance.OpenLoadingPlane();

    }

    //挑战
    private void ChallengeClick()
    {
        //加载场景
        UIManagement.Instance.sceneName = "DailyChallenge";
        UIManagement.Instance.OpenLoadingPlane();
    }

    //图鉴
    private void IllustrationsClick()
    {
        //打开图鉴界面
    }

    //装扮
    private void PretendClick()
    {
        //打开装扮界面
    }

    //设置界面
    private void SettingClick()
    {
        UIManagement.Instance.OpenSettingPlane();
    }

    public void ClosePlane()
    {
        bottom_OBJ.DOMoveY(-300, 0.3f);
    }
}
