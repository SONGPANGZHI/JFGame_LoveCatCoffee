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

    public GameObject blockPrefab;

    private void Start()
    {
        //AddTempBlock();
        BlockPropData.JudgeScendRowUnlockActon += CheckMiddleData;
        BlockPropData.JudgeThirdRowUnlockActon += CheckTopData;
    }


    //生成 第一条 传送带
    public void CreateLeftConveyor(int ID)
    {
        MiddleBlock(GameLevelManagement.Instance.middleBlockDic_Top[ID]);
        TopBolck(GameLevelManagement.Instance.topBlockDic_Top[ID]);
        BottomBolck(GameLevelManagement.Instance.bottomBlockDic_Top[ID]);
        Invoke("UnlockBlockData", 0.2F);

    }

    public void CreateRightConveyor(int ID)
    {
        MiddleBlock(GameLevelManagement.Instance.middleBlockDic_Bottom[ID]);
        TopBolck(GameLevelManagement.Instance.topBlockDic_Bottom[ID]);
        BottomBolck(GameLevelManagement.Instance.bottomBlockDic_Bottom[ID]);
        Invoke("UnlockBlockData", 0.2F);
    }
    //生成Top
    public void TopBolck(List<BlockPropDataClass> blockList)
    {
        CreateBlock(blockList, top_Tran, BlockHierarchy.TopBlock);
    }

    //中间
    public void MiddleBlock(List<BlockPropDataClass> blockList)
    {
        CreateBlock(blockList, middle_Tran, BlockHierarchy.MiddleBlock);

    }

    //底部
    public void BottomBolck(List<BlockPropDataClass> blockList)
    {
        CreateBlock(blockList, bottom_Tran, BlockHierarchy.BottomBlock);

    }

    //透视道具使用生成方块
    public void RandomBlock()
    {
       
    }

    //生成方块
    public void CreateBlock(List<BlockPropDataClass> blockPropDatas,Transform trans, BlockHierarchy blockHierarchy)
    {
        for (int i = 0; i < blockPropDatas.Count; i++)
        {
            GameObject GO = Instantiate(blockPrefab, trans);
            GO.name = blockPropDatas[i].config.blockPropType.ToString() + blockPropDatas[i].ID;
            GO.GetComponent<BlockPropData>().hierarchy = blockHierarchy;
            GO.GetComponent<BlockPropData>().BlockInit(blockPropDatas[i]);
            GO.GetComponent<RectTransform>().anchoredPosition = new Vector2(i * horizontalSpacing, 0);
            
        }
    }

   

    #region 添加数据

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

    //检查是否解锁
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

    #endregion

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

    private void OnDisable()
    {
        BlockPropData.JudgeScendRowUnlockActon -= CheckMiddleData;
        BlockPropData.JudgeThirdRowUnlockActon -= CheckTopData;
    }
}
