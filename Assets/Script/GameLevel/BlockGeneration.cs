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

    public int layerID;

    private List<BlockPropData> topBlockList = new List<BlockPropData>();
    private List<BlockPropData> middleBlockList = new List<BlockPropData>();
    private List<BlockPropData> bottomBlockList = new List<BlockPropData>();


   

    private List<BlockPropData> blockAll_Temp = new List<BlockPropData>();

    private bool isStartCheck;
    private void Start()
    {
        //AddTempBlock();
        BlockPropData.JudgeScendRowUnlockActon += CheckMiddleData;
        BlockPropData.JudgeThirdRowUnlockActon += CheckTopData;
    }

    //初始化 方块
    public void InitBlock()
    {
    }

    //生成 第一条 传送带
    public void CreateLeftConveyor(int ID)
    {
        MiddleBlock(2, GameLevelManagement.Instance.middleBlockDic_Top[ID], GameLevelManagement.Instance.middleBlockDic_Bool_Top[ID],ConveyorLayer.Top);
        TopBolck(1, GameLevelManagement.Instance.topBlockDic_Top[ID], GameLevelManagement.Instance.topBlockDic_Bool_Top[ID], ConveyorLayer.Top);
        BottomBolck(3, GameLevelManagement.Instance.bottomBlockDic_Top[ID], GameLevelManagement.Instance.bottomBlockDic_Bool_Top[ID], ConveyorLayer.Top);
        Invoke("UnlockBlockData", 0.2F);

    }

    public void CreateRightConveyor(int ID)
    {
        MiddleBlock(ID, GameLevelManagement.Instance.middleBlockDic_Bottom[ID], GameLevelManagement.Instance.middleBlockDic_Bool_Bottom[ID], ConveyorLayer.Bottom);
        TopBolck(ID, GameLevelManagement.Instance.topBlockDic_Bottom[ID], GameLevelManagement.Instance.topBlockDic_Bool_Bottom[ID], ConveyorLayer.Bottom);
        BottomBolck(ID, GameLevelManagement.Instance.bottomBlockDic_Bottom[ID], GameLevelManagement.Instance.bottomBlockDic_Bool_Bottom[ID], ConveyorLayer.Bottom);
        Invoke("UnlockBlockData", 0.2F);
    }
    //生成Top
    public void TopBolck(int id, List<BlockPropData> blockList, List<bool> bools, ConveyorLayer conveyorLayer)
    {
        if (GameLevelManagement.Instance.perspective)
            RandomBlock(id,blockList, top_Tran, BlockHierarchy.TopBlock, bools, conveyorLayer);
        else
            CreateBlock(id,blockList, top_Tran, BlockHierarchy.TopBlock, bools, conveyorLayer);
    }

    //中间
    public void MiddleBlock(int id, List<BlockPropData> blockList,List<bool> bools,ConveyorLayer conveyorLayer)
    {

        if (GameLevelManagement.Instance.perspective)
            RandomBlock(id,blockList, middle_Tran, BlockHierarchy.MiddleBlock, bools, conveyorLayer);
        else
            CreateBlock(id,blockList, middle_Tran, BlockHierarchy.MiddleBlock, bools, conveyorLayer);
        //for (int i = 0; i < middleBlockList.Count; i++)
        //{
        //    GameObject GO = Instantiate(middleBlockList[i].prefab, middle_Tran);
        //    GO.GetComponent<BlockPropData>().blockHierarchy = BlockHierarchy.MiddleBlock;
        //    GO.GetComponent<BlockPropData>().InitBlock();
        //    GO.GetComponent<RectTransform>().anchoredPosition = new Vector2(i * horizontalSpacing, 0);
        //}
    }

    //底部
    public void BottomBolck(int id,List<BlockPropData> blockList, List<bool> bools, ConveyorLayer conveyorLayer)
    {
        if (GameLevelManagement.Instance.perspective)
            RandomBlock(id,blockList, bottom_Tran, BlockHierarchy.BottomBlock, bools, conveyorLayer);
        else
            CreateBlock(id,blockList, bottom_Tran, BlockHierarchy.BottomBlock, bools, conveyorLayer);
        //for (int i = 0; i < bottomBlockList.Count; i++)
        //{
        //    GameObject GO = Instantiate(bottomBlockList[i].prefab, bottom_Tran);
        //    GO.GetComponent<BlockPropData>().blockHierarchy = BlockHierarchy.BottomBlock;
        //    GO.GetComponent<BlockPropData>().InitBlock();
        //    GO.GetComponent<RectTransform>().anchoredPosition = new Vector2(i * horizontalSpacing, 0);
        //}
    }

    //透视道具使用生成方块
    public void RandomBlock(int id,List<BlockPropData> blockPropDatas, Transform trans, BlockHierarchy blockHierarchy, List<bool> bools,ConveyorLayer conveyorLayer)
    {
        for (int i = 0; i < blockPropDatas.Count; i++)
        {
            if (blockPropDatas[i].blockPropType == BlockPropType.Gift)
            {
                int blockID = Random.Range(0, GameLevelManagement.Instance.blockPropAll.Count - 1);
                GameObject GO = Instantiate(GameLevelManagement.Instance.blockPropAll[blockID].prefab, trans);
                GO.GetComponent<BlockPropData>().blockHierarchy = blockHierarchy;
                GO.GetComponent<BlockPropData>().conveyorLayer = conveyorLayer;
                //GO.GetComponent<BlockPropData>()._isActive = bools[i];
                GO.GetComponent<BlockPropData>().InitBlock(id,i, bools[i], layerID);
                GO.GetComponent<RectTransform>().anchoredPosition = new Vector2(i * horizontalSpacing, 0);
            }
            else
            {
                GameObject GO = Instantiate(blockPropDatas[i].prefab, trans);
                GO.GetComponent<BlockPropData>().blockHierarchy = blockHierarchy;
                GO.GetComponent<BlockPropData>().conveyorLayer = conveyorLayer;
                //GO.GetComponent<BlockPropData>()._isActive = bools[i];
                GO.GetComponent<BlockPropData>().InitBlock(id,i, bools[i], layerID);
                GO.GetComponent<RectTransform>().anchoredPosition = new Vector2(i * horizontalSpacing, 0);
            }
        }
    }

    //生成方块
    public void CreateBlock(int id,List<BlockPropData> blockPropDatas,Transform trans, BlockHierarchy blockHierarchy,List<bool> bools, ConveyorLayer conveyorLayer)
    {
        if (conveyorLayer == ConveyorLayer.Top)
        {
            switch (blockHierarchy)
            {
                case BlockHierarchy.TopBlock:
                    id = 0;
                    break;
                case BlockHierarchy.MiddleBlock:
                    id = 1;
                    break;
                case BlockHierarchy.BottomBlock:
                    id = 2;
                    break;
            }
        }
        else
        {
            switch (blockHierarchy)
            {
                case BlockHierarchy.TopBlock:
                    id = 0;
                    break;
                case BlockHierarchy.MiddleBlock:
                    id = 1;
                    break;
                case BlockHierarchy.BottomBlock:
                    id = 2;
                    break;
            }
        }


      
        for (int i = 0; i < blockPropDatas.Count; i++)
        {
            GameObject GO = Instantiate(blockPropDatas[i].prefab, trans);
            GO.GetComponent<BlockPropData>().blockHierarchy = blockHierarchy;
            GO.GetComponent<BlockPropData>().conveyorLayer = conveyorLayer;
            //GO.GetComponent<BlockPropData>()._isActive = bools[i];
            GO.GetComponent<BlockPropData>().InitBlock(id,i, bools[i], layerID);
            GO.GetComponent<RectTransform>().anchoredPosition = new Vector2(i * horizontalSpacing, 0);
        }
    }

    //检查已生成的道具是否有礼盒
    //public void CheckGiftBlock()
    //{
    //    for (int i = 0; i < top_Tran.childCount; i++)
    //    {
    //        if (top_Tran.GetChild(i).GetComponent<BlockPropData>().blockPropType == BlockPropType.Gift)
    //        {
    //            Destroy(top_Tran.GetChild(i).gameObject);
    //            int blockID = Random.Range(0, GameLevelManagement.Instance.blockPropAll.Count - 1);
    //            GameObject GO = Instantiate(GameLevelManagement.Instance.blockPropAll[blockID].prefab, top_Tran);
    //            GO.transform.SetSiblingIndex(i);
    //            GO.GetComponent<BlockPropData>().blockHierarchy = BlockHierarchy.TopBlock;
    //            GO.GetComponent<BlockPropData>().InitBlock();
    //            GO.GetComponent<RectTransform>().anchoredPosition = new Vector2(i * horizontalSpacing, 0);
    //        }
    //    }

    //    for (int i = 0; i < middle_Tran.childCount; i++)
    //    {
    //        if (middle_Tran.GetChild(i).GetComponent<BlockPropData>().blockPropType == BlockPropType.Gift)
    //        {
    //            Destroy(middle_Tran.GetChild(i).gameObject);
    //            int blockID = Random.Range(0, GameLevelManagement.Instance.blockPropAll.Count - 1);
    //            GameObject GO = Instantiate(GameLevelManagement.Instance.blockPropAll[blockID].prefab, middle_Tran);
    //            GO.transform.SetSiblingIndex(i);
    //            GO.GetComponent<BlockPropData>().blockHierarchy = BlockHierarchy.MiddleBlock;
    //            GO.GetComponent<BlockPropData>().InitBlock();
    //            GO.GetComponent<RectTransform>().anchoredPosition = new Vector2(i * horizontalSpacing, 0);
    //        }
    //    }

    //    for (int i = 0; i < bottom_Tran.childCount; i++)
    //    {
    //        if (bottom_Tran.GetChild(i).GetComponent<BlockPropData>().blockPropType == BlockPropType.Gift)
    //        {
    //            Destroy(bottom_Tran.GetChild(i).gameObject);
    //            int blockID = Random.Range(0, GameLevelManagement.Instance.blockPropAll.Count - 1);
    //            GameObject GO = Instantiate(GameLevelManagement.Instance.blockPropAll[blockID].prefab, bottom_Tran);
    //            GO.transform.SetSiblingIndex(i);
    //            GO.GetComponent<BlockPropData>().blockHierarchy = BlockHierarchy.BottomBlock;
    //            GO.GetComponent<BlockPropData>().InitBlock();
    //            GO.GetComponent<RectTransform>().anchoredPosition = new Vector2(i * horizontalSpacing, 0);
    //        }
    //    }
    //}


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

    //分三个数组
    public void AllocatingArrays()
    {
        GameLevelManagement.Instance.blockPropData_Temp.Shuffle();
        for (int i = 0; i < GameLevelManagement.Instance.blockPropData_Temp.Count; i++)
        {
            if (i < 6)
            {
                topBlockList.Add(GameLevelManagement.Instance.blockPropData_Temp[i]);
            }
            else if (i >= 6 && i < 12)
            {
                middleBlockList.Add(GameLevelManagement.Instance.blockPropData_Temp[i]);
            }
            else
            {
                bottomBlockList.Add(GameLevelManagement.Instance.blockPropData_Temp[i]);
            }
        }
    }

    private void OnDisable()
    {
        BlockPropData.JudgeScendRowUnlockActon -= CheckMiddleData;
        BlockPropData.JudgeThirdRowUnlockActon -= CheckTopData;

    }



}
