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

    public GameObject defualtBG_IMG;

    public Image saveImage_IMG;

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
        OpenSaveImage();
        bottom_OBJ.DOMoveY(300,0.3f);
        currentLevel_TMP.text = "当前关卡：" + (PlayerPrefs.GetInt(GameManager.CurrentGameLevelKey));
        if (PlayerPrefs.HasKey(UIManagement.redPointKey))
            OpenRedPiont();
        else
            CloseRedPonit();

        DetermineOpenDress();
    }

    //开始
    private void StartPlayClick()
    {
        //加载场景
        UIManagement.Instance._isChallengBool = false;
        MusicManagement.instance.ClickPlaySFX();
        UIManagement.Instance.sceneName = "NewPlayGame";
        GameManager.Instance.SavaChallengTime();
        UIManagement.Instance.OpenLoadingPlane();

    }

    //挑战
    private void ChallengeClick()
    {
        //加载场景
        //UIManagement.Instance._isChallengBool = true;
        //MusicManagement.instance.ClickPlaySFX();
        //UIManagement.Instance.sceneName = "GameLevel";
        //UIManagement.Instance.OpenLoadingPlane();
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
        UIManagement.Instance.sceneName = "DressUp";
        UIManagement.Instance.OpenLoadingPlane();
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

    //判断是否有本地保存图片
    public void OpenSaveImage()
    {
        if (PlayerPrefs.HasKey(GameManager.SaveImageKey))
        {
            //本地存在图片
            Texture2D tex = BaseTools.Instance.GetCurrentPhoto();
            defualtBG_IMG.SetActive(false);
            Sprite sprite = Sprite.Create(
                tex,
                new Rect(0, 0, tex.width, tex.height),
                new Vector2(0.5f, 0.5f)
            );

            saveImage_IMG.sprite = sprite;
            saveImage_IMG.gameObject.SetActive(true);
        }
        else
        {
            //使用默认的
            defualtBG_IMG.SetActive(true);
            saveImage_IMG.gameObject.SetActive(false);
        }

    }
}
