using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGeneration : MonoBehaviour
{
    public Transform top_Tran;
    public Transform middle_Tran;
    public Transform bottom_Tran;
    public float horizontalSpacing = 2f;


    private List<BlockPropData> topBlockList = new List<BlockPropData>();
    private List<BlockPropData> middleBlockList = new List<BlockPropData>();
    private List<BlockPropData> bottomBlockList = new List<BlockPropData>();


    //初始化 方块
    public void InitBlock()
    {
        AllocatingArrays();
        BottomBolck();
        MiddleBlock();
        TopBolck();
    }

    //生成Top
    public void TopBolck()
    {
        for (int i = 0; i < topBlockList.Count; i++)
        {
            GameObject GO = Instantiate(topBlockList[i].prefab,top_Tran);
            GO.GetComponent<BlockPropData>().blockHierarchy = BlockHierarchy.TopBlock;
            //UnlockBlockData_Three(GO.GetComponent<BlockPropData>());
            GO.GetComponent<BlockPropData>().InitBlock();
            GO.GetComponent<RectTransform>().anchoredPosition = new Vector2(i* horizontalSpacing,0);
        }
    }

    //中间
    public void MiddleBlock()
    {
        for (int i = 0; i < middleBlockList.Count; i++)
        {
            GameObject GO = Instantiate(middleBlockList[i].prefab, middle_Tran);
            GO.GetComponent<BlockPropData>().blockHierarchy = BlockHierarchy.MiddleBlock;
            //UnlockBlockData_Two(GO.GetComponent<BlockPropData>(),i);
            GO.GetComponent<BlockPropData>().InitBlock();
            GO.GetComponent<RectTransform>().anchoredPosition = new Vector2(i * horizontalSpacing, 0);
        }
    }

    //底部
    public void BottomBolck()
    {
        for (int i = 0; i < bottomBlockList.Count; i++)
        {
            GameObject GO = Instantiate(bottomBlockList[i].prefab, bottom_Tran);
            GO.GetComponent<BlockPropData>().blockHierarchy = BlockHierarchy.BottomBlock;
            GO.GetComponent<BlockPropData>().InitBlock();
            GO.GetComponent<RectTransform>().anchoredPosition = new Vector2(i * horizontalSpacing, 0);
        }
    }

    //添加数据 第二排
    public void UnlockBlockData_Two(BlockPropData _blockPropData,int ID)
    {
        _blockPropData.unlockblock.Add(bottomBlockList[ID].prefab);

        if(ID+1<= bottomBlockList.Count)
            _blockPropData.unlockblock.Add(bottomBlockList[ID+1].prefab);

    }

    //添加数据 第二排
    public void UnlockBlockData_Three(BlockPropData _blockPropData)
    {
        for (int i = 0; i < topBlockList.Count; i++)
        {
            if (i > 0 && i < middleBlockList.Count)
            {
                if (_blockPropData.unlockblock.Count > 2)
                    return;
                _blockPropData.unlockblock.Add(middleBlockList[i - 1].gameObject);
                _blockPropData.unlockblock.Add(middleBlockList[i].gameObject);
            }
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

        topBlockList.Clear();
        middleBlockList.Clear();
        bottomBlockList.Clear();

    }

    //分三个数组
    public void AllocatingArrays()
    {
        GameManager.Instance.blockPropAll.Shuffle();
        //if (GameManager.Instance.blockPropAll.Count > 18)
        //{
        //    GameManager.Instance.blockPropAll.Remove(GameManager.Instance.blockPropAll[0]);
        //    GameManager.Instance.blockPropAll.Remove(GameManager.Instance.blockPropAll[1]);
        //}
        //GameManager.Instance.blockPropAll.Add(GameManager.Instance.blockPropAll[RandomID()]);
        //GameManager.Instance.blockPropAll.Add(GameManager.Instance.blockPropAll[RandomID()]);

        for (int i = 0; i < GameManager.Instance.blockPropAll.Count; i++)
        {
            if (i < 6)
            {
                topBlockList.Add(GameManager.Instance.blockPropAll[i]);
            }
            else if (i >= 6 && i < 12)
            {
                middleBlockList.Add(GameManager.Instance.blockPropAll[i]);
            }
            else
            {
                bottomBlockList.Add(GameManager.Instance.blockPropAll[i]);
            }
        }
    }

    //随机一个数
    public int RandomID()
    {
        int ID = Random.Range(0, GameManager.Instance.blockPropAll.Count);
        return ID;
    }
}
