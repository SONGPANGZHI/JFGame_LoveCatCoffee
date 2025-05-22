using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayGameManagement : MonoBehaviour
{
    public static PlayGameManagement Instance;

    public RectTransform uiTrans;
    public RectTransform middleTrans;

    [Header("放置区数据")]
    public List<GameObject> dropZoneData;
    public Transform dropZoneTran;
    public GameObject dropZonePrefab;

    public List<BlockDataConfigNew> blockDataConfig;         
    public GameObject blockPrefab;

    [Header("猫猫需求")]
    public int allMiddleBlockNum = 0;

    public List<CatData> catDataAll;
    public List<CatData> catNeedBlock;
    private CatData catData_Temp;

    [Header("传送带 速度")]
    public bool keepTime = false;
    private float timer;

    [Header("道具速度 存在时长 默认30s")]
    public float speedSurvivalTime = 30F;

    [Header("透视道具")]
    public bool perspective = false;
    private float perspectiveTimer = 0;
    [Header("透视道具 存在时长 默认30s")]
    public float perspectiveSurvivalTime = 30f;

    [Header("关卡数据")]
    public List<BlockDataConfigNew> blockTypes;
    public int middleMin;
    public int middleMax;
    public float conveyorSpeed;
    public int positionsNum;
    public int blockTypeNum;
    public int blockArea;
    public int conveyorArea;
    public List<string> furnitureName;

    public int middleAllNum;

    public bool giftsPercentProgress_60;
    public bool giftsPercentProgress_80;
    public bool giftsPercentProgress_100;

    public List<BlockPropData> currentMysteryBox;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;

        //获取方块列表
        GetBlockTypeList();

        if (UIManagement.Instance._isChallengBool)
        {
            //每日挑战
            GameManager.Instance.GetGameLevelData_TEMP(30);
            GitMiddleAreaData();
        }
        else
        {
            //关卡
            GameManager.Instance.GetGameLevelData();
            GitMiddleAreaData();
        }

        GameManager.Instance.pauseGame = true;

    }

    private void Start()
    {
        BaseTools.Instance.UIAdaptive(uiTrans,middleTrans);
    }

    #region 三消逻辑
    public void CreateDropZoneObject(BlockDataConfigNew _blockProp, bool middle)
    {
        GameObject currentOBJ = Instantiate(dropZonePrefab, dropZoneTran);
        currentOBJ.GetComponent<DropZone>().DropZoneInitNew(_blockProp, middle);

        dropZoneData.Add(currentOBJ);

        CheckForMatches();

        //if (CatNeedBlock(_blockProp))
        //    catData_Temp.UpdateTMP();
    }

    //检查物品类型
    public void CheckForMatches()
    {
        // 获取所有卡牌并按类型分组
        var cardGroups = dropZoneData
            .OrderBy(card => card.GetComponent<DropZone>().blockPropTypeNew)  // 先按类型排序
            .GroupBy(card => card.GetComponent<DropZone>().blockPropTypeNew)  // 然后分组
            .Where(group => group.Count() >= 3);  // 筛选出数量>=3的组

        if (cardGroups.Count() == 0)
            Invoke("DetermineDropAreaFull", 0.3f);

        // 处理匹配的卡牌组
        foreach (var group in cardGroups)
        {
            // 获取前三个匹配的卡牌
            var matchedCards = group.Take(3).ToList();

            //CreateParticle(matchedCards);
            // 销毁卡牌或执行消除动画
            StartCoroutine(DestroyObject(matchedCards));

            // 可以在这里添加得分逻辑等
            //Debug.Log($"消除了3个{group.Key}类型的卡牌");
            //Invoke("DetermineDropAreaFull", 0.5f);
        }

        // 重新排列剩余卡牌
        RearrangeCards();

    }

    //重新排列
    private void RearrangeCards()
    {
        // 1. 获取所有卡牌并缓存信息
        var cardsWithInfo = dropZoneData.Select(card => new {
            Card = card,
            Type = card.GetComponent<DropZone>().blockPropTypeNew,
            OriginalIndex = card.transform.GetSiblingIndex()
        }).ToList();

        // 2. 按类型分组并记录每个类型的最后位置
        var typeLastIndexDict = new Dictionary<BlockPropTypeNew, int>();
        foreach (var card in cardsWithInfo.OrderBy(c => c.OriginalIndex))
        {
            typeLastIndexDict[card.Type] = cardsWithInfo.IndexOf(card);
        }

        // 3. 排序规则：
        //    - 先按类型第一次出现的位置排序（保持类型组的相对顺序）
        //    - 同类型组内按原始顺序排序
        var sortedCards = cardsWithInfo
            .OrderBy(card => typeLastIndexDict.Keys.ToList().IndexOf(card.Type))
            .ThenBy(card => card.OriginalIndex)
            .Select(card => card.Card)
            .ToList();

        // 4. 重新设置顺序
        for (int i = 0; i < sortedCards.Count; i++)
        {
            sortedCards[i].transform.SetSiblingIndex(i);
        }
    }

    //检查放置区 中间牌
    public bool CheckDorpZoneMiddleBlock()
    {
        for (int i = 0; i < dropZoneData.Count; i++)
        {
            if (dropZoneData[i].GetComponent<DropZone>().isMiddle)
            {
                return false;
            }
        }
        return true;
    }

    //0.3秒后销毁
    IEnumerator DestroyObject(List<GameObject> matchedCards)
    {

        yield return new WaitForSeconds(0.3f);
        foreach (var card in matchedCards)
        {
            dropZoneData.Remove(card);
            Destroy(card.gameObject);
        }
        Invoke("DetermineDropAreaFull", 0.5f);
    }

    //检查游戏状态
    public void DetermineDropAreaFull()
    {
        if (dropZoneData.Count >= 7)
        {
            //游戏结束逻辑
            GameManager.Instance.pauseGame = false;
            UIManagement.Instance.OpenGameOverPlane();
        }
        CheckeGameOver();
    }

    //判断是否结束
    public void CheckeGameOver()
    {
        if (middleAllNum <= 0 && CheckDorpZoneMiddleBlock())
        {
            //游戏结束
            UIManagement.Instance.OpenGameOverPlane(true);
           
        }

    }

    #endregion

    #region 关卡数据

    //获取方块列表
    public void GetBlockTypeList()
    {
        blockTypes.Clear();

        for (int i = 0; i < blockDataConfig.Count; i++)
        {
            blockTypes.Add(blockDataConfig[i]);
        }
        blockTypes.Shuffle();
    }

    //获取关卡详细信息
    public void GitMiddleAreaData()
    {
        middleMin = GameManager.Instance.currentGameLevel.BlockNum.min;
        middleMax = GameManager.Instance.currentGameLevel.BlockNum.max;
        conveyorSpeed = GameManager.Instance.currentGameLevel.conveyorSpeed;
        positionsNum = GameManager.Instance.currentGameLevel.PositionsNum;
        blockTypeNum = GameManager.Instance.currentGameLevel.BlockType;
        conveyorArea = (int)(GameManager.Instance.currentGameLevel.MysteryBox.ConveyorArea * 10);
        blockArea = (int)(GameManager.Instance.currentGameLevel.MysteryBox.BlockArea * 10);
        if (GameManager.Instance.currentGameLevel.LevelID >= 17)
        {
            //奖励池随机抽取1-3家具
            int awardNum = Random.Range(1,4);
            Debug.LogError("awardNum ;" + awardNum);
            RadomGetAwardFurniture(awardNum);
        }
        else
            furnitureName = GameManager.Instance.currentGameLevel.FurnitureName;

        
    }

    //获得奖励
    public List<string> RadomGetAwardFurniture(int IndexID)
    {
        GameManager.Instance.CurrentData.AwardFurniturePool.Shuffle();

        for (int i = 0; i < IndexID; i++)
        {
            furnitureName.Add(GameManager.Instance.CurrentData.AwardFurniturePool[i]);
        }

        return furnitureName;
    }


    #endregion

    ////猫咪需求
    //public bool CatNeedBlock(BlockDataConfigNew _blockProp)
    //{
    //    for (int i = 0; i < catNeedBlock.Count; i++)
    //    {
    //        if (_blockProp.blockPropType == catNeedBlock[i].needBlock.blockPropType)
    //        {
    //            catData_Temp = catNeedBlock[i];
    //            return true;
    //        }
    //    }
    //    return false;
    //}


    ////检查猫咪需求
    //public void CheckCatRequirements(CatData catData)
    //{
    //    for (int i = 0; i < dropZoneData.Count; i++)
    //    {
    //        if (dropZoneData[i].GetComponent<DropZone>().blockPropTypeNew == catData.needBlock.blockPropType)
    //        {
    //            catData.UpdateTMP();
    //        }
    //    }
    //}


    #region 道具的使用

    //清除道具
    public void ClearPropUse()
    {
        dropZoneData.Clear();
        for (int i = 0; i < dropZoneTran.childCount; i++)
        {
            Destroy(dropZoneTran.GetChild(i).gameObject);
        }
        //检查游戏状态
        CheckeGameOver();
    }

    //加速道具使用
    public void SpeedPropUse()
    {
        conveyorSpeed = 0.7f;
        keepTime = true;
        Debug.LogError("开始加速 当前速度 0.7");
    }

    //速度计时器
    public void SpeedTimer()
    {
        if (keepTime)
        {
            timer += Time.deltaTime;
            if (timer >= speedSurvivalTime)
            {
                keepTime = false;
                conveyorSpeed = 0.2f;
                timer = 0;
                Debug.LogError("加速结束 当前速度 0.3");
            }
        }
    }

    //透视道具使用
    public void PerspectivePropUse()
    {
        perspective = true;

        for (int i = 0; i < currentMysteryBox.Count; i++)
        {
            if (currentMysteryBox[i] == null)
            {
                currentMysteryBox.RemoveAt(i);
                continue;
            }
            else
            {
                currentMysteryBox[i].CloseMysteryBox();
            }
            
        }
        Debug.LogError("透视开始");

    }

    //打开盲盒
    public void OpenMysteryBoxShow()
    {
        for (int i = 0; i < currentMysteryBox.Count; i++)
        {
            if (currentMysteryBox[i].midlleBlock)
                currentMysteryBox[i].OpenMaysteryBox();
        }
    }

    //盲盒列表刷新
    public void UpdateMysteryBox()
    {
        //for (int i = 0; i < currentMysteryBox.Count; i++)
        //{
        //    if (currentMysteryBox[i] == null)
        //        currentMysteryBox.RemoveAt(i);
        //}
        currentMysteryBox.RemoveAll(item => item == null);
    }

    //透视倒计时
    public void PerspectiveTimer()
    {
        if (perspective)
        {
            perspectiveTimer += Time.deltaTime;
            if (perspectiveTimer >= perspectiveSurvivalTime)
            {
                perspective = false;
                perspectiveTimer = 0;
                OpenMysteryBoxShow();
                Debug.LogError("透视结束");
            }
        }
    }

    #endregion

    private void Update()
    {
        SpeedTimer();
        PerspectiveTimer();
    }
}
