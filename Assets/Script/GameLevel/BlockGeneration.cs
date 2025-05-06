using System;
using System.Collections.Generic;
using UnityEngine;

public static class ListExtensionsTEMP
{
    private static System.Random rng = new System.Random();

    public static void ShuffleTemp<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}

public class BlockGeneration : MonoBehaviour
{
    #region 新版玩法三消
    
    public List<Transform> prefabTrans;
   

    //生成所有数据
    public void GenerateAllBlock()
    {
        for (int i = 0; i < prefabTrans.Count; i++)
        {
            int elementCount = UnityEngine.Random.Range(1,4);
            GenerateBlock(i, elementCount);
        }
    }

    //生成一组数据
    public void GenerateBlock(int transIndex,int count)
    {
        PlayGameManagement.Instance.blockDataConfig_TEMP.ShuffleTemp();
        for (int i = 0; i < count; i++)
        {
            GameObject element = Instantiate(PlayGameManagement.Instance.blockPrefab, prefabTrans[transIndex]);
            element.transform.localPosition = new Vector2(0, i * 15);
            element.GetComponent<BlockPropData>().BlockInit(PlayGameManagement.Instance.blockDataConfig_TEMP[i]);

            if (i < count - 1)
                element.GetComponent<BlockPropData>().ButtonNotClickable();

            
            PlayGameManagement.Instance.currentSceneBlock.Add(element.GetComponent<BlockPropData>());
        }
    }

    public void ClearAllBlock()
    {
        for (int i = 0; i < prefabTrans.Count; i++)
        {
            for (int j = 0; j < prefabTrans[i].childCount; j++)
            {
                Destroy(prefabTrans[i].GetChild(j).gameObject);
            }
        }
    }

    #endregion




    #region 旧版玩法三消
    //public Transform top_Tran;
    //public Transform middle_Tran;
    //public Transform bottom_Tran;
    //public float horizontalSpacing = 2f;

    //private List<BlockPropData> topBlockList = new List<BlockPropData>();
    //private List<BlockPropData> middleBlockList = new List<BlockPropData>();
    //private List<BlockPropData> bottomBlockList = new List<BlockPropData>();

    //public GameObject blockPrefab;

    //private void Start()
    //{
    //    //AddTempBlock();
    //    BlockPropData.JudgeScendRowUnlockActon += CheckMiddleData;
    //    BlockPropData.JudgeThirdRowUnlockActon += CheckTopData;
    //}


    ////生成 第一条 传送带
    //public void CreateLeftConveyor(int ID)
    //{
    //    MiddleBlock(GameLevelManagement.Instance.middleBlockDic_Top[ID]);
    //    TopBolck(GameLevelManagement.Instance.topBlockDic_Top[ID]);
    //    BottomBolck(GameLevelManagement.Instance.bottomBlockDic_Top[ID]);
    //    Invoke("UnlockBlockData", 0.1F);

    //}

    //public void CreateRightConveyor(int ID)
    //{
    //    MiddleBlock(GameLevelManagement.Instance.middleBlockDic_Bottom[ID]);
    //    TopBolck(GameLevelManagement.Instance.topBlockDic_Bottom[ID]);
    //    BottomBolck(GameLevelManagement.Instance.bottomBlockDic_Bottom[ID]);
    //    Invoke("UnlockBlockData", 0.1F);
    //}
    ////生成Top
    //public void TopBolck(List<BlockPropDataClass> blockList)
    //{
    //    CreateBlock(blockList, top_Tran, BlockHierarchy.TopBlock);
    //}

    ////中间
    //public void MiddleBlock(List<BlockPropDataClass> blockList)
    //{
    //    CreateBlock(blockList, middle_Tran, BlockHierarchy.MiddleBlock);

    //}

    ////底部
    //public void BottomBolck(List<BlockPropDataClass> blockList)
    //{
    //    CreateBlock(blockList, bottom_Tran, BlockHierarchy.BottomBlock);

    //}

    ////透视道具使用生成方块
    //public void RandomBlock()
    //{

    //}

    ////生成方块
    //public void CreateBlock(List<BlockPropDataClass> blockPropDatas,Transform trans, BlockHierarchy blockHierarchy)
    //{
    //    for (int i = 0; i < blockPropDatas.Count; i++)
    //    {
    //        GameObject GO = Instantiate(blockPrefab, trans);
    //        GO.name = blockPropDatas[i].config.blockPropType.ToString() + blockPropDatas[i].ID;
    //        GO.GetComponent<BlockPropData>().hierarchy = blockHierarchy;
    //        GO.GetComponent<BlockPropData>().BlockInit(blockPropDatas[i]);
    //        GO.GetComponent<RectTransform>().anchoredPosition = new Vector2(i * horizontalSpacing, 0);

    //    }
    //}



    //#region 添加数据

    ////添加数据 第二排  bottom->middle
    //public void UnlockBlockData()
    //{
    //    for (int i = 0; i < bottom_Tran.childCount; i++)
    //    {
    //        if (i == 0)
    //            middle_Tran.GetChild(i).GetComponent<BlockPropData>().unlockblock.Add(bottom_Tran.GetChild(i).gameObject);
    //        else
    //        {
    //            middle_Tran.GetChild(i).GetComponent<BlockPropData>().unlockblock.Add(bottom_Tran.GetChild(i - 1).gameObject);
    //            middle_Tran.GetChild(i).GetComponent<BlockPropData>().unlockblock.Add(bottom_Tran.GetChild(i).gameObject);
    //        }
    //    }

    //    for (int i = 0; i < middle_Tran.childCount; i++)
    //    {
    //        if (i == middle_Tran.childCount - 1)
    //            top_Tran.GetChild(i).GetComponent<BlockPropData>().unlockblock.Add(middle_Tran.GetChild(i).gameObject);
    //        else
    //        {
    //            top_Tran.GetChild(i).GetComponent<BlockPropData>().unlockblock.Add(middle_Tran.GetChild(i).gameObject);
    //            top_Tran.GetChild(i).GetComponent<BlockPropData>().unlockblock.Add(middle_Tran.GetChild(i + 1).gameObject);
    //        }
    //    }

    //}

    ////检查第一层数据
    //public void CheckMiddleData()
    //{
    //    for (int i = 0; i < middle_Tran.childCount; i++)
    //    {
    //        if (UnlockBlock(middle_Tran.GetChild(i).GetComponent<BlockPropData>()))
    //            middle_Tran.GetChild(i).GetComponent<BlockPropData>().ButtonClickable();
    //    }
    //}

    ////检查第二层数据
    //public void CheckTopData()
    //{
    //    for (int i = 0; i < top_Tran.childCount; i++)
    //    {
    //        if (UnlockBlock(top_Tran.GetChild(i).GetComponent<BlockPropData>()))
    //            top_Tran.GetChild(i).GetComponent<BlockPropData>().ButtonClickable();
    //    }
    //}

    ////检查是否解锁
    //public bool UnlockBlock(BlockPropData blockPropData)
    //{
    //    foreach (var item in blockPropData.unlockblock)
    //    {
    //        if (item.activeSelf)
    //        {
    //            return false;
    //        }
    //    }
    //    return true;
    //}

    //#endregion

    ////清空
    //public void ClearAllObject()
    //{
    //    topBlockList.Clear();
    //    middleBlockList.Clear();
    //    bottomBlockList.Clear();

    //    for (int i = 0; i < bottom_Tran.childCount; i++)
    //    {
    //        Destroy(bottom_Tran.GetChild(i).gameObject);
    //    }
    //    for (int i = 0; i < middle_Tran.childCount; i++)
    //    {
    //        Destroy(middle_Tran.GetChild(i).gameObject);
    //    }
    //    for (int i = 0; i < top_Tran.childCount; i++)
    //    {
    //        Destroy(top_Tran.GetChild(i).gameObject);
    //    }
    //}

    //private void OnDisable()
    //{
    //    BlockPropData.JudgeScendRowUnlockActon -= CheckMiddleData;
    //    BlockPropData.JudgeThirdRowUnlockActon -= CheckTopData;
    //}

    #endregion
}
