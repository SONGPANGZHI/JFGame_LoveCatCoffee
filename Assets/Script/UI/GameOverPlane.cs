using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPlane : MonoBehaviour
{
    [SerializeField]
    private TMP_Text challengesNum_TMP;             //今日挑战文本
    [SerializeField]
    private TMP_Text LevelProgress_TMP;             //关卡进度 文本
    [SerializeField]
    private Image scheduleVlue;                     //进度条

    [SerializeField]
    private GameObject defeated_UI;
    [SerializeField]
    private GameObject victory_UI;

    [SerializeField]
    private Image gift_60;                          //关卡进度60% 礼物
    [SerializeField]
    private Image gift_80;                          //关卡进度80% 礼物
    [SerializeField]
    private Image gift_100;                         //关卡礼物100% 礼物

    [SerializeField]
    private List<Sprite> openGift_IMG;              //打开礼物的Sprite 0 = 60 , 1 = 80 , 2 = 100 

    [SerializeField]    
    private Button resChallenge_BTN;               //重玩按钮
    [SerializeField]
    private Button back_BTN;                        //返回按钮

    private float progress;

    private bool planeState;


    #region  文本常量 不需要变动
    private const string VictoryChallengesNum_TMP = "今日挑战<NUM>次";
    private const string DefeatedChallengesNum_TMP = "今日已挑战<NUM>次";
    private const string ProgressBarNum_TMP = "关卡进度<NUM>%";
    #endregion

    private void Awake()
    {
        resChallenge_BTN.onClick.AddListener(RecChanllengeClick);
        back_BTN.onClick.AddListener(BackMain);
    }

    //界面初始化  _isVictory = true 打开胜利界面  false 失败界面
    public void GameOverPlaneInit(bool _isVictory = false)
    {
        planeState = _isVictory;
        transform.GetChild(0).DOScale(new Vector3(1, 1, 1), 0.3F);

        JudgingPlaneState();

        JudgingGameProgress();

    }

    //判断界面状态
    public void JudgingPlaneState()
    {
        if (planeState)
        {
            //胜利
            resChallenge_BTN.gameObject.SetActive(false);
            victory_UI.SetActive(true);
            defeated_UI.SetActive(false);
            challengesNum_TMP.text = GameManager.Instance.GetNumbersText(VictoryChallengesNum_TMP, GameManager.Instance.NumberLevelChallenges);
            GameManager.Instance.SavaChallengTime();
        }
        else
        {
            //失败
            resChallenge_BTN.gameObject.SetActive(true);
            defeated_UI.SetActive(true);
            victory_UI.SetActive(false);
            challengesNum_TMP.text = GameManager.Instance.GetNumbersText(DefeatedChallengesNum_TMP, GameManager.Instance.NumberLevelChallenges);
        }

        LevelProgress_TMP.text = GameManager.Instance.GetNumbersText(ProgressBarNum_TMP, GetGameProgress());
    }

    //获得游戏进度
    public int GetGameProgress()
    {
        int currentProgress = PlayGameManagement.Instance.allMiddleBlockNum - PlayGameManagement.Instance.middleAllNum;
        float progree = (currentProgress / PlayGameManagement.Instance.allMiddleBlockNum) * 100;
        progress = progree / 100;
        return (int)progree;
    }

    //判断游戏进度
    public void JudgingGameProgress()
    {
        scheduleVlue.fillAmount = 0;
        if (progress == 1)
        {
            //打开 奖励领取
            scheduleVlue.DOFillAmount(1, 0.3f).SetEase(Ease.Linear).OnComplete(() =>
            {
                UIManagement.Instance.OpenAwardPlane();
            });
        }
        else
        {
            scheduleVlue.DOFillAmount(progress, 0.3f).SetEase(Ease.Linear);
        }
    }

    //进度条 
    public void SetProgress(float value)
    {
        scheduleVlue.DOFillAmount(value, 0.3f).SetEase(Ease.Linear).OnComplete(() =>
        {
            UIManagement.Instance.OpenAwardPlane();
        });
    }

    //重新挑战
    public void RecChanllengeClick()
    {
        MusicManagement.instance.ClickPlaySFX();
        GameManager.Instance.SavaChallengTime();
        transform.GetChild(0).DOScale(new Vector3(0, 0, 0), 0.3F).OnComplete(() =>
        {
            //加载界面
            UIManagement.Instance.CloseGamePlane();
            this.gameObject.SetActive(false);
            UIManagement.Instance.OpenLoadingPlane();
            //if(planeState)
            //    GameLevelManagement.Instance.currentLevelData = GameLevelManagement.Instance.gameLevelDataList[PlayerPrefs.GetInt(GameManager.CurrentGameLevelKey) - 1];
        });
    }

    //返回主界面
    public void BackMain()
    {
        MusicManagement.instance.ClickPlaySFX();
        transform.GetChild(0).DOScale(new Vector3(0, 0, 0), 0.3F).OnComplete(() =>
        {
            this.gameObject.SetActive(false);
        });
        UIManagement.Instance.loadingPlane.gameObject.SetActive(true);
        UIManagement.Instance.CloseGamePlane();
        UIManagement.Instance.loadingPlane.LoadUIScene();
    }

}
