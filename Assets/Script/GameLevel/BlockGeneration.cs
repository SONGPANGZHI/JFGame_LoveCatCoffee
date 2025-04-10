using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    private List<BlockPropData> blockAll_Temp = new List<BlockPropData>();
   
    private void Start()
    {
        //AddTempBlock();
        BlockPropData.JudgeScendRowUnlockActon += CheckMiddleData;
        BlockPropData.JudgeThirdRowUnlockActon += CheckTopData;
    }

    //初始化 方块
    public void InitBlock()
    {
        AllocatingArrays();
        
        MiddleBlock();
        TopBolck();
        BottomBolck();

        Invoke("UnlockBlockData",0.3F);

        //UnlockBlockData();
    }

    //生成Top
    public void TopBolck()
    {
        for (int i = 0; i < topBlockList.Count; i++)
        {
            GameObject GO = Instantiate(topBlockList[i].prefab,top_Tran);
            GO.GetComponent<BlockPropData>().blockHierarchy = BlockHierarchy.TopBlock;
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

    //添加数据 第二排  bottom->middle
    public void UnlockBlockData()
    {
        for (int i = 0; i < bottom_Tran.childCount; i++)
        {
            if (i == 0)
                middle_Tran.GetChild(i).GetComponent<BlockPropData>().unlockblock.Add(bottom_Tran.GetChild(i).gameObject);
            else
            {
                middle_Tran.GetChild(i).GetComponent<BlockPropData>().unlockblock.Add(bottom_Tran.GetChild(i - 1).gameObject);
                middle_Tran.GetChild(i).GetComponent<BlockPropData>().unlockblock.Add(bottom_Tran.GetChild(i).gameObject);
            }
        }

        for (int i = 0; i < middle_Tran.childCount; i++)
        {
            if (i == middle_Tran.childCount - 1)
                top_Tran.GetChild(i).GetComponent<BlockPropData>().unlockblock.Add(middle_Tran.GetChild(i).gameObject);
            else
            {
                top_Tran.GetChild(i).GetComponent<BlockPropData>().unlockblock.Add(middle_Tran.GetChild(i).gameObject);
                top_Tran.GetChild(i).GetComponent<BlockPropData>().unlockblock.Add(middle_Tran.GetChild(i + 1).gameObject);
            }
        }

    }

    //检查第一层数据
    public void CheckMiddleData()
    {
        for (int i = 0; i < middle_Tran.childCount; i++)
        {
            if (UnlockBlock(middle_Tran.GetChild(i).GetComponent<BlockPropData>()))
                middle_Tran.GetChild(i).GetComponent<BlockPropData>().ButtonClickable();
        }
    }

    //检查第二层数据
    public void CheckTopData()
    {
        for (int i = 0; i < top_Tran.childCount; i++)
        {
            if (UnlockBlock(top_Tran.GetChild(i).GetComponent<BlockPropData>()))
                top_Tran.GetChild(i).GetComponent<BlockPropData>().ButtonClickable();
        }
    }

    public bool UnlockBlock(BlockPropData blockPropData)
    {
        foreach (var item in blockPropData.unlockblock)
        {
            if (item.activeSelf)
            {
                return false;
            }
        }
        return true;
    }

    //清空
    public void ClearAllObject()
    {
        topBlockList.Clear();
        middleBlockList.Clear();
        bottomBlockList.Clear();

        for (int i = 0; i < bottom_Tran.childCount; i++)
        {
            Destroy(bottom_Tran.GetChild(i).gameObject);
        }
        for (int i = 0; i < middle_Tran.childCount; i++)
        {
            Destroy(middle_Tran.GetChild(i).gameObject);
        }
        for (int i = 0; i < top_Tran.childCount; i++)
        {
            Destroy(top_Tran.GetChild(i).gameObject);
        }
    }

    //分三个数组
    public void AllocatingArrays()
    {
        GameLevelManagement.Instance.blockPropAll.Shuffle();

        for (int i = 0; i < GameLevelManagement.Instance.blockPropAll.Count; i++)
        {
            if (i < 6)
            {
                topBlockList.Add(GameLevelManagement.Instance.blockPropAll[i]);
            }
            else if (i >= 6 && i < 12)
            {
                middleBlockList.Add(GameLevelManagement.Instance.blockPropAll[i]);
            }
            else
            {
                bottomBlockList.Add(GameLevelManagement.Instance.blockPropAll[i]);
            }
        }
    }

    //随机一个数
    public int RandomID()
    {
        int ID = Random.Range(0, GameLevelManagement.Instance.blockPropAll.Count);
        return ID;
    }

    public void AddTempBlock()
    {
        for (int i = 0; i < GameLevelManagement.Instance.blockPropAll.Count; i++)
        {
            blockAll_Temp.Add(GameLevelManagement.Instance.blockPropAll[i]);
        }
    }
}
