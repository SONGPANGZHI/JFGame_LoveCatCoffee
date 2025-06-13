using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReminderBox : MonoBehaviour
{
    private string unlockGridStr = "解锁成功！";
    private string increaseDifficultyStr = "难度增加！";
    private string timer_30 = "获得60S加时！";

    public TMP_Text tips_TMP;
    public Image tips_IMG;

    //打开界面
    public void OpenReminderBox(int ID = 0)
    {
        if (ID == 0)
            tips_TMP.text = unlockGridStr;
        else if(ID == 2)
            tips_TMP.text = timer_30;
        else
            tips_TMP.text = increaseDifficultyStr;

        tips_IMG.gameObject.SetActive(true);
        tips_IMG.DOFade(1, 0.2f).SetEase(Ease.Linear);
        tips_TMP.DOFade(1, 0.2f).SetEase(Ease.Linear);

        //关闭界面
        StartCoroutine(CloseReminderBox());
    }

    //关闭界面
    IEnumerator CloseReminderBox()
    {
        yield return new WaitForSeconds(1f);

        tips_IMG.DOFade(0, 0.2f).SetEase(Ease.Linear);
        tips_TMP.DOFade(0, 0.2f).SetEase(Ease.Linear).OnComplete(() => 
        {
            tips_IMG.gameObject.SetActive(false);
        });
    }
}
