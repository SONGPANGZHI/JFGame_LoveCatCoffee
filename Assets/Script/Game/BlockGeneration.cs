using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGeneration : MonoBehaviour
{
    public Transform top_Tran;
    public Transform middle_Tran;
    public Transform bottom_Tran;
    public float horizontalSpacing = 2f;

    
    //初始化 方块
    public void InitBlock()
    {
        GameManager.Instance.blockPropAll.Shuffle();
        TopBolck();
        MiddleBlock();
        BottomBolck();
    }

    //生成Top
    public void TopBolck()
    {
        for (int i = 0; i < 6; i++)
        {
            GameObject GO = Instantiate(GameManager.Instance.blockPropAll[i].prefab,top_Tran);
            GO.GetComponent<RectTransform>().anchoredPosition = new Vector2(i* horizontalSpacing,0);
        }
    }

    //中间
    public void MiddleBlock()
    {
        for (int i = 6; i < 12; i++)
        {
            GameObject GO = Instantiate(GameManager.Instance.blockPropAll[i].prefab, middle_Tran);
            GO.GetComponent<RectTransform>().anchoredPosition = new Vector2((i-6) * horizontalSpacing, 0);
        }
    }

    //底部
    public void BottomBolck()
    {
        GameManager.Instance.blockPropAll.Shuffle();
        for (int i = 0; i < 6; i++)
        {
            GameObject GO = Instantiate(GameManager.Instance.blockPropAll[i].prefab, bottom_Tran);
            GO.GetComponent<RectTransform>().anchoredPosition = new Vector2(i * horizontalSpacing, 0);
        }
    }

    //清空
    public void ClearAllObject()
    {
        for (int i = 0; i < top_Tran.childCount; i++)
        {
            Destroy(top_Tran.GetChild(i).gameObject);
        }
        for (int i = 0; i < middle_Tran.childCount; i++)
        {
            Destroy(middle_Tran.GetChild(i).gameObject);
        }
        for (int i = 0; i < bottom_Tran.childCount; i++)
        {
            Destroy(bottom_Tran.GetChild(i).gameObject);
        }
    }
}
