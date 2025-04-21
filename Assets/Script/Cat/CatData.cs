using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CatData : MonoBehaviour
{
    public BlockDataConfig needBlock;

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
        GameLevelManagement.Instance.catNeedBlock.Add(this);
        needNum_TMP.gameObject.SetActive(false);
    }

    //随机道具
    public BlockDataConfig RandomBlockProp()
    {
        GameLevelManagement.Instance.AddCatNeedID();
        needBlock = GameLevelManagement.Instance.needCatData_Temp[GameLevelManagement.Instance.catNeedBlockID];
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
            GameLevelManagement.Instance.catNeedBlock.Remove(this);
            GameManager.Instance.currentNumberCats += 1;
            UIManagement.Instance.gamePlane.TargetTmpChange();
            StartCoroutine(DestroyObject());
        }
    }

    //销毁该目标
    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(1F);
        Destroy(gameObject);

        if (GameManager.Instance.currentNumberCats < GameLevelManagement.Instance.currentLevelData.Target - 2)
        {
            CreateCatAction?.Invoke();

        }
        else if (GameManager.Instance.currentNumberCats == GameLevelManagement.Instance.currentLevelData.Target)
        {
            //游戏结束
            UIManagement.Instance.OpenGameOverPlane(true);
            GameManager.Instance.SavaGameLevel();
        }
           
        
    }

    //public void AddCatNeedBlock()
    //{
    //    for (int i = 0; i < GameLevelManagement.Instance.blockPropAll.Count - 1; i++)
    //    {
    //        catNeedBlock_Temp.Add(GameLevelManagement.Instance.blockPropAll[i]);
    //    }
    //}
}
