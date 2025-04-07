using DG.Tweening;
using Newtonsoft.Json.Linq;
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

    public float progress;

    private void Awake()
    {
        resChallenge_BTN.onClick.AddListener(RecChanllengeClick);
        back_BTN.onClick.AddListener(BackMain);
    }

    //界面初始化
    public void GameOverPlaneInit()
    {
        transform.GetChild(0).DOScale(new Vector3(1,1,1),0.3F);
        AwardPlane.OnRewardTwo += GetSecondReward;
        AwardPlane.OnRewardThree += GetThirdReward;
        AwardPlane.OnRewardOne += ResidueProgress;
        JudgingGameProgress();
    }


    //判断游戏进度
    public void JudgingGameProgress()
    {
        scheduleVlue.fillAmount = 0;
        if (progress >= 0.6)
        {
            //领取礼盒 第一阶段
            SetProgress(0.6f);
            if (progress >= 0.6 && progress < 0.8)
            {
                AwardPlane.getGift_60 = true;
            }
            else
            {
                AwardPlane.getGift_80 = true;
            }
        }
        else
        {
            scheduleVlue.DOFillAmount(progress, 0.3f).SetEase(Ease.Linear);
        }
    }

    //未达到 礼物领取 进度条动画
    public void ResidueProgress()
    {
        float current = scheduleVlue.fillAmount;
        float interpolation = progress - current;

        if (progress > 0.6 && progress < 0.8)
        {
            scheduleVlue.DOFillAmount(current + interpolation, 0.3f).SetEase(Ease.Linear);
        }
        else if (progress > 0.8 && progress < 1)
        {
            scheduleVlue.DOFillAmount(current + interpolation, 0.3f).SetEase(Ease.Linear);
        }
    }


    //奖励界面关闭 第二次奖励界面
    public void GetSecondReward()
    {
        float current = scheduleVlue.fillAmount;
        if (progress >= 0.8)
        {
            SetProgress(0.8F);
            if (progress >= 0.8 && progress == 1)
            {
                AwardPlane.getGift_100 = true;
            }
            else
                AwardPlane.getGift_60 = true;
        }
        else
        {
            AwardPlane.getGift_60 = true;
            float value = progress - 0.6f;
            scheduleVlue.DOFillAmount(current + value, 0.3f).SetEase(Ease.Linear);
        }
    }

    //领取第三次奖励
    public void GetThirdReward()
    {
        float current = scheduleVlue.fillAmount;
        if (progress == 1)
        {
            //开启第二阶段 礼物领取
            SetProgress(1);
        }
        else
        {
            AwardPlane.getGift_60 = true;
            float value = progress - 0.8f;
            scheduleVlue.DOFillAmount(current + value, 0.3f).SetEase(Ease.Linear);
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
        transform.GetChild(0).DOScale(new Vector3(0, 0, 0), 0.3F).OnComplete(() =>
        {
            //加载界面
            UIManagement.Instance.CloseGamePlane();
            this.gameObject.SetActive(false);
            UIManagement.Instance.OpenLoadingPlane();
        });
    }

    //返回主界面
    public void BackMain()
    {
        transform.GetChild(0).DOScale(new Vector3(0, 0, 0), 0.3F).OnComplete(() =>
        {
            AwardPlane.OnRewardTwo -= GetSecondReward;
            AwardPlane.OnRewardThree -= GetThirdReward;
            AwardPlane.OnRewardOne -= ResidueProgress;
            this.gameObject.SetActive(false);
        });
        UIManagement.Instance.loadingPlane.gameObject.SetActive(true);
        UIManagement.Instance.CloseGamePlane();
        UIManagement.Instance.loadingPlane.LoadUIScene();
    }


    //#region 测试
    ////成功界面
    //public void OpenVictoey()
    //{ 
    //    victory_UI.SetActive(true);
    //    defeated_UI.SetActive(false);
    //    challengesNum_TMP.text = "今日已挑战1次";
    //    LevelProgress_TMP.text = "关卡进度" + 100;

    //}
    //#endregion
}
