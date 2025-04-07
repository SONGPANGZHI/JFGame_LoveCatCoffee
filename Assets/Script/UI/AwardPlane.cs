using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;


public class AwardPlane : MonoBehaviour
{
    [SerializeField]
    private Image icon;
    [SerializeField]
    private Button ok_BTN;

    public static Action OnRewardTwo;
    public static Action OnRewardThree;
    public static Action OnRewardOne;

    public static bool getGift_80;
    public static bool getGift_100;
    public static bool getGift_60;
    private void Awake()
    {
        ok_BTN.onClick.AddListener(OKClick);
    }

    //界面初始化
    public void AwardPlaneInit()
    {
        transform.GetChild(0).DOScale(new Vector3(1,1,1),0.7f);
    }

    //三个阶段回调
    public void OKClick()
    {
        transform.GetChild(0).DOScale(new Vector3(0,0,0), 0.7f).OnComplete(() =>
        {
            this.gameObject.SetActive(false);

            if (getGift_60)
            {
                OnRewardOne?.Invoke();
                getGift_60 = false;
            }
            else if (getGift_80)
            {
                OnRewardTwo?.Invoke();
                getGift_80 = false;
            }
            else if (getGift_100)
            {
                OnRewardThree?.Invoke();
                getGift_100 = false;
            }
        });
    }

}
