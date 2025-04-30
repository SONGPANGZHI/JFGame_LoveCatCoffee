using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CatData : MonoBehaviour
{
    public BlockDataConfigNew needBlock;

    public Transform dialogue_OBJ;

    public TMP_Text needNum_TMP;

    public Image propIcon_IMG;

    public GameObject finish_IMG;


    public int text_NUM = 3;

    public static Action CreateCatAction;

    

    //初始化
    public void InitCatData()
    {
        finish_IMG.SetActive(false);
        RandomBlockProp();
        propIcon_IMG.sprite = needBlock.Icon;
        propIcon_IMG.SetNativeSize();
        PlayGameManagement.Instance.catNeedBlock.Add(this);
        needNum_TMP.gameObject.SetActive(false);
    }

    //随机道具
    public BlockDataConfigNew RandomBlockProp()
    {
        //GameLevelManagement.Instance.AddCatNeedID();
        //needBlock = GameLevelManagement.Instance.needCatData_Temp[GameLevelManagement.Instance.catNeedBlockID];
        int randomID = UnityEngine.Random.Range(0, text_NUM);
        needBlock = PlayGameManagement.Instance.blockDataConfig_TEMP[randomID];
        return needBlock;
    }

    //随机对话方向
    public void RandomDialogueDirection()
    {
        int randomID = UnityEngine.Random.Range(0,2);
        if (randomID == 0)
        {
            //对话向左.
            dialogue_OBJ.GetComponent<RectTransform>().anchoredPosition = new Vector3(-50, -20, 0);
            dialogue_OBJ.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            //对话向右
            dialogue_OBJ.GetComponent<RectTransform>().anchoredPosition = new Vector3(50, -20, 0);
            dialogue_OBJ.localScale = new Vector3(1, 1, 1);
        }
    }

    //更新文本
    public void UpdateTMP()
    {
        text_NUM -= 1;
        if (text_NUM == 0)
        {
            finish_IMG.SetActive(true);
            //needNum_TMP.gameObject.SetActive(true);
            PlayGameManagement.Instance.catNeedBlock.Remove(this);
            GameManager.Instance.currentNumberCats += 1;
            UIManagement.Instance.gamePlane.TargetTmpChange();
            StartCoroutine(DestroyObject());
        }
    }

    //销毁该目标
    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(0.3F);
        Destroy(gameObject);

        //if (PlayGameManagement.Instance.middleAllNum <= 0)
        //{
        //    //游戏结束
        //    UIManagement.Instance.OpenGameOverPlane(true);
        //    GameManager.Instance.SavaGameLevel();
        //}
        //else
        //{
        //    CreateCatAction?.Invoke();
        //}


        if (GameManager.Instance.currentNumberCats < PlayGameManagement.Instance.catTarget - 2)
        {

            CreateCatAction?.Invoke();
        }
        //else if (GameManager.Instance.currentNumberCats == PlayGameManagement.Instance.catTarget)
        //{
        //    //游戏结束
        //    UIManagement.Instance.OpenGameOverPlane(true);
        //    GameManager.Instance.SavaGameLevel();
        //}
    }

}
