using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CatData : MonoBehaviour
{
    public BlockPropData needBlock;

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
        propIcon_IMG.sprite = needBlock.GetComponent<Image>().sprite;
        propIcon_IMG.SetNativeSize();
        GameLevelManagement.Instance.catNeedBlock.Add(this);
        needNum_TMP.gameObject.SetActive(false);
        //RandomDialogueDirection();
    }

    //随机道具
    public BlockPropData RandomBlockProp()
    {
        int propID = UnityEngine.Random.Range(0, GameLevelManagement.Instance.blockPropData_Temp.Count-1);
        needBlock = GameLevelManagement.Instance.blockPropData_Temp[propID];
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

            StartCoroutine(DestroyObject());
        }
        //else
        //{
        //    needNum_TMP.text = text_NUM.ToString();
        //}
    }

    //销毁该目标
    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(1F);
        Destroy(gameObject);
        GameManager.Instance.currentNumberCats += 1;
        if(GameManager.Instance.currentNumberCats <= 30)
            CreateCatAction?.Invoke();
    }

    //public void AddCatNeedBlock()
    //{
    //    for (int i = 0; i < GameLevelManagement.Instance.blockPropAll.Count - 1; i++)
    //    {
    //        catNeedBlock_Temp.Add(GameLevelManagement.Instance.blockPropAll[i]);
    //    }
    //}
}
