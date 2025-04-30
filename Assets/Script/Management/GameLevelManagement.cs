using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GameLevelManagement : MonoBehaviour
{
    public static GameLevelManagement Instance;
    [Header("关卡数据")]
    public List<GameLevelConfig> gameLevelDataList;

    [Header("方块道具种类")]
    public List<BlockDataConfig> blockPropAll;

    [Header("猫咪种类")]
    public List<CatData> catDataAll;

    [Header("猫咪需求道具")]
    public List<CatData> catNeedBlock;

    [Header("放置区数据")]
    public List<GameObject> dropZoneData;
    public Transform dropZoneTran;
    public GameObject dropZonePrefab;

    private CatData catData_Temp;
    private GameObject currentOBJ;

    [Header("传送带 速度")]
    public float conveyorSpeed = 0.2f;
    public bool keepTime = false;
    private float timer;

    [Header("道具速度 存在时长 默认30s")]
    public float speedSurvivalTime = 30F ;

    [Header("透视道具")]
    public bool perspective = false;
    private float perspectiveTimer;
    [Header("透视道具 存在时长 默认30s")]
    public float perspectiveSurvivalTime = 30f;


    [Header("关卡数据")]
    public List<BlockDataConfig> blockPropData_Temp;      //临时数据
    public GameLevelConfig currentLevelData;
    public bool _isNovice;

    public GameObject destroyParticle;

    public Dictionary<int, List<BlockPropDataClass>> topBlockDic_Top = new Dictionary<int, List<BlockPropDataClass>>();
    public Dictionary<int, List<BlockPropDataClass>> middleBlockDic_Top = new Dictionary<int, List<BlockPropDataClass>>();
    public Dictionary<int, List<BlockPropDataClass>> bottomBlockDic_Top = new Dictionary<int, List<BlockPropDataClass>>();

    public Dictionary<int, List<BlockPropDataClass>> topBlockDic_Bottom = new Dictionary<int, List<BlockPropDataClass>>();
    public Dictionary<int, List<BlockPropDataClass>> middleBlockDic_Bottom = new Dictionary<int, List<BlockPropDataClass>>();
    public Dictionary<int, List<BlockPropDataClass>> bottomBlockDic_Bottom = new Dictionary<int, List<BlockPropDataClass>>();

    public Dictionary<int, BlockPropDataClass> currentAllBlockData = new Dictionary<int, BlockPropDataClass>();

    public GameObject prefab;
    public Transform conveyor;
    public Transform dorpZonePos;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        LoadGameLevel();
    }

    private void Start()
    {
        GameManager.Instance.pauseGame = true;
        GameManager.Instance.currentNumberCats = 0;
    }

    #region 获取关卡数据

    public List<BlockDataConfig> keyCardData;
    public List<BlockDataConfig> SurplusCardData;

    public List<BlockPropDataClass> topLayerData_Temp;
    public List<BlockPropDataClass> middleLayerData_Temp;
    public List<BlockPropDataClass> bottomLayerData_Temp;

    public List<BlockDataConfig> needCatData_Temp;            //猫咪需求牌
    public List<BlockDataConfig> SurplusCardType;             //其余牌的种类 类型


    public int eachLayerNum;                            //每一层数据 总数/3
    public int keyCardAllNum;
    public int keyType;
    public int masteryNum;

    private int topLayerKeyCardNum;
    private int middleLayerKeyCardNum;
    private int bottomLayerKeyCardNum;

    private int middleLayerSurplusCardNum;
    private int bottomLayerSurplusCardNum;
    private int topLayerSurplusCardNum;

    //加载 关卡数据
    public void LoadGameLevel()
    {
        if (PlayerPrefs.GetInt(GameManager.CurrentGameLevelKey) == 0)
        {
            //新手关卡
            _isNovice = true;
        }
        else
        {
            _isNovice = false;
        }

        if (UIManagement.Instance._isChallengBool)
        {
            _isNovice = false;
            //挑战
            currentLevelData = gameLevelDataList[19];
        }
        else
        {
            //每日
            currentLevelData = gameLevelDataList[PlayerPrefs.GetInt(GameManager.CurrentGameLevelKey)];
        }

        //currentLevelData = gameLevelDataList[PlayerPrefs.GetInt(GameManager.CurrentGameLevelKey)];
        blockPropData_Temp.Shuffle();                                           //随机
        keyType = currentLevelData.TypeID / 2;                                  //关键牌 种类
        eachLayerNum = currentLevelData.Amount / 3;                             //每一行 关键牌 总数
        masteryNum = currentLevelData.Amount - currentLevelData.KeyCardNum;     //冗余牌 总数
        AllocatingRowsKeyCard();

        //分类猫咪需求牌和冗余牌
        AddCatNeedType();
        //添加 关键牌 和 冗余牌
        GetKeyCard();
        MysteryCard();
        //添加 到总字典
        AddAllBlockDataDic();

        //添加每一行牌数
        GetKeyCardLayer();
        GetSurplusCardLayer();

        //添加到 字典
        //InitEachLayerBlockList();

        SegmentationTopData();
        SegmentationMiddleData();
        SegmentationBottomData();

    }

    //分配各行 关键牌数
    public void AllocatingRowsKeyCard()
    {
        middleLayerKeyCardNum = (int)Math.Ceiling(currentLevelData.KeyCardNum * currentLevelData.CardType[0].InterLayer);       
        topLayerKeyCardNum = (int)Math.Ceiling(currentLevelData.KeyCardNum * currentLevelData.CardType[0].TopLayer);
        bottomLayerKeyCardNum = currentLevelData.KeyCardNum - middleLayerKeyCardNum - topLayerKeyCardNum;


        middleLayerSurplusCardNum = eachLayerNum - middleLayerKeyCardNum;                     
        bottomLayerSurplusCardNum = eachLayerNum - bottomLayerKeyCardNum;                     
        topLayerSurplusCardNum = eachLayerNum - topLayerKeyCardNum;


    }


    //添加猫咪需求种类
    public void AddCatNeedType()
    {
        //添加猫咪需求牌 关键牌种类数
        for (int i = 0; i < keyType; i++)
        {
            needCatData_Temp.Add(blockPropData_Temp[i]);
        }

        //剩余分配到 冗余牌
        for (int i = keyType; i < currentLevelData.TypeID; i++)
        {
            SurplusCardType.Add(blockPropData_Temp[i]);
        }
    }

    //获取 
    public void GetKeyCard()
    {
        //添加关键牌
        for (int i = 0; i < currentLevelData.KeyCardNum; i++)
        {
            if (keyCardData.Count >= currentLevelData.KeyCardNum)
                return;

            if (needCatData_Temp.Count <= i)
            {
                i = 0;
                keyCardData.Add(needCatData_Temp[i]);
            }
            else
            {
                keyCardData.Add(needCatData_Temp[i]);
            }
        }
    }

    //冗余牌
    public void MysteryCard()
    {
        for (int i = 0; i < masteryNum; i++)
        {
            if (SurplusCardData.Count >= masteryNum)
                return;

            if (SurplusCardType.Count <= i)
            {
                i = 0;
                SurplusCardData.Add(SurplusCardType[i]);
            }
            else
            {
                SurplusCardData.Add(SurplusCardType[i]);
            }
        }
    }

    /// <summary>
    /// 获得关键牌在每层排列
    /// </summary>
    public void GetKeyCardLayer()
    {
        for (int i = 0; i < topLayerKeyCardNum; i++)
        {
            topLayerData_Temp.Add(currentAllBlockData[i]);
        }

        for (int i = 0; i < middleLayerKeyCardNum; i++)
        {
            middleLayerData_Temp.Add(currentAllBlockData[topLayerKeyCardNum + i]);
        }

        int otherID = topLayerKeyCardNum + middleLayerKeyCardNum;

        for (int i = 0; i < bottomLayerKeyCardNum; i++)
        {
            bottomLayerData_Temp.Add(currentAllBlockData[otherID + i]);
        }
    }

    /// <summary>
    /// 其他牌 在每一层分部
    /// </summary>
    public void GetSurplusCardLayer()
    {
        for (int i = 0; i < bottomLayerSurplusCardNum; i++)
        {
            bottomLayerData_Temp.Add(currentAllBlockData[i + keyCardData.Count]);
        }
        int middleID = keyCardData.Count + bottomLayerSurplusCardNum;
        for (int i = 0; i < middleLayerSurplusCardNum; i++)
        {
            middleLayerData_Temp.Add(currentAllBlockData[i + middleID]);
        }
        int bottomID = keyCardData.Count + bottomLayerSurplusCardNum + middleLayerSurplusCardNum;

        for (int i = 0; i < topLayerSurplusCardNum; i++)
        {
            topLayerData_Temp.Add(currentAllBlockData[i + bottomID]);
        }

        topLayerData_Temp.Shuffle();
        middleLayerData_Temp.Shuffle();
        bottomLayerData_Temp.Shuffle();
    }


    //分割top数据
    public void SegmentationTopData()
    {
        topLayerData_Temp.Shuffle();
        List<BlockPropDataClass> topLayerData_Top_Temp = new List<BlockPropDataClass>();
        List<BlockPropDataClass> topLayerData_Bottom_Temp = new List<BlockPropDataClass>();


        int ID = topLayerData_Temp.Count / 2;

        for (int i = 0; i < topLayerData_Temp.Count; i++)
        {
            if (i < ID)
                topLayerData_Top_Temp.Add(topLayerData_Temp[i]);
            else
            {
                topLayerData_Bottom_Temp.Add(topLayerData_Temp[i]);
            }
        }

        int arrayID = topLayerData_Top_Temp.Count / 6;
        topBlockDic_Top = topLayerData_Top_Temp.SplitIntoGroups(arrayID);
        topBlockDic_Bottom = topLayerData_Bottom_Temp.SplitIntoGroups(arrayID);

    }

    //分割 middle 数组
    public void SegmentationMiddleData()
    {
        middleLayerData_Temp.Shuffle();
        List<BlockPropDataClass> topLayerData_Top_Temp = new List<BlockPropDataClass>();
        List<BlockPropDataClass> topLayerData_Bottom_Temp = new List<BlockPropDataClass>();
        
        int ID = topLayerData_Temp.Count / 2;

        for (int i = 0; i < middleLayerData_Temp.Count; i++)
        {
            if (i < ID)
                topLayerData_Top_Temp.Add(middleLayerData_Temp[i]);
            else
                topLayerData_Bottom_Temp.Add(middleLayerData_Temp[i]);
        }
        int arrayID = topLayerData_Top_Temp.Count / 6;
        middleBlockDic_Top = topLayerData_Top_Temp.SplitIntoGroups(arrayID);
        middleBlockDic_Bottom = topLayerData_Bottom_Temp.SplitIntoGroups(arrayID);
    }

    //分割 bottom 数组
    public void SegmentationBottomData()
    {
        bottomLayerData_Temp.Shuffle();
        List<BlockPropDataClass> topLayerData_Top_Temp = new List<BlockPropDataClass>();
        List<BlockPropDataClass> topLayerData_Bottom_Temp = new List<BlockPropDataClass>();
       
        int ID = topLayerData_Temp.Count / 2;

        for (int i = 0; i < bottomLayerData_Temp.Count; i++)
        {
            if (i < ID)
                topLayerData_Top_Temp.Add(bottomLayerData_Temp[i]);
            else
                topLayerData_Bottom_Temp.Add(bottomLayerData_Temp[i]);
        }
        int arrayID = topLayerData_Top_Temp.Count / 6;
        bottomBlockDic_Top = topLayerData_Top_Temp.SplitIntoGroups(arrayID);
        bottomBlockDic_Bottom = topLayerData_Bottom_Temp.SplitIntoGroups(arrayID);
    }


    /// <summary>
    /// 把所有牌 添加到字典
    /// </summary>
    public void AddAllBlockDataDic()
    {
        for (int i = 0; i < keyCardData.Count; i++)
        {
            BlockPropDataClass propData = new BlockPropDataClass(i, true, keyCardData[i]);

            currentAllBlockData.Add(i, propData);
        }

        for (int i = keyCardData.Count; i < currentLevelData.Amount; i++)
        {
            BlockPropDataClass propData = new BlockPropDataClass(i,true, SurplusCardData[i - keyCardData.Count]);
            currentAllBlockData.Add(i, propData);
        }

    }

   
    /// <summary>
    /// 根据Key 修改当前Bool值
    /// </summary>
    /// <param name="dictKey"></param>
    /// <param name="newData"></param>
    public void ModifyBlockByIndex(int dictKey, bool newData)
    {
        currentAllBlockData[dictKey].active = newData;
    }

    #endregion


    #region  放置区以及三消逻辑

    //生成放置区物品
    public void CreateDropZoneObject(BlockPropData _blockProp)
    {
        //int randomID = UnityEngine.Random.Range(0, blockPropAll.Count - 1);
        //currentOBJ = Instantiate(dropZonePrefab, dropZoneTran);
        //if (_blockProp.propType == BlockPropType.Gift)
        //{
        //    currentOBJ.GetComponent<DropZone>().DropZoneInit(blockPropAll[randomID]);
        //    currentOBJ.name = blockPropAll[randomID].blockPropType.ToString();
        //}
        //else
        //{
        //    currentOBJ.GetComponent<DropZone>().DropZoneInit(_blockProp.blockPropData.config);
        //    currentOBJ.name = _blockProp.propType.ToString();
        //}

        //dropZoneData.Add(currentOBJ);

        //CheckForMatches();

        //if (CatNeedBlock(_blockProp))
        //    catData_Temp.UpdateTMP();

        
    }

    //检查物品类型
    public void CheckForMatches()
    {
        // 获取所有卡牌并按类型分组
        var cardGroups = dropZoneData
            .OrderBy(card => card.GetComponent<DropZone>().blockPropType)  // 先按类型排序
            .GroupBy(card => card.GetComponent<DropZone>().blockPropType)  // 然后分组
            .Where(group => group.Count() >= 3);  // 筛选出数量>=3的组

        if(cardGroups.Count() == 0)
            Invoke("DetermineDropAreaFull", 0.3f);
        
        // 处理匹配的卡牌组
        foreach (var group in cardGroups)
        {
            // 获取前三个匹配的卡牌
            var matchedCards = group.Take(3).ToList();

            CreateParticle(matchedCards);
            // 销毁卡牌或执行消除动画
            StartCoroutine(DestroyObject(matchedCards));
            
            // 可以在这里添加得分逻辑等
            Debug.Log($"消除了3个{group.Key}类型的卡牌");
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
            Type = card.GetComponent<DropZone>().blockPropType,
            OriginalIndex = card.transform.GetSiblingIndex()
        }).ToList();

        // 2. 按类型分组并记录每个类型的最后位置
        var typeLastIndexDict = new Dictionary<BlockPropType, int>();
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

    //检查游戏状态
    public void DetermineDropAreaFull()
    {
        if (dropZoneData.Count >= 7)
        {
            //游戏结束逻辑
            GameManager.Instance.pauseGame = false;
            UIManagement.Instance.OpenGameOverPlane();
        }
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

    //生成粒子特效
    public void CreateParticle(List<GameObject> matchedCards)
    {
        foreach (var item in matchedCards)
        {
            GameObject GO = Instantiate(destroyParticle, item.transform);
            StartCoroutine(DestoryParticle(GO));
        }
    }

    //销毁粒子
    IEnumerator DestoryParticle(GameObject _Particle)
    {
        yield return new WaitForSeconds(1f);
        Destroy(_Particle);
    }


    #endregion

    #region  猫咪需求

    public int catNeedBlockID = -1;

    public void AddCatNeedID()
    {
        catNeedBlockID += 1;

        if (catNeedBlockID > needCatData_Temp.Count - 1)
        {
            catNeedBlockID = 0;
        }
        

    }

    //猫咪需求
    public bool CatNeedBlock(BlockPropData _catBlock)
    {
        //for (int i = 0; i < catNeedBlock.Count; i++)
        //{
        //    if (_catBlock.propType == catNeedBlock[i].needBlock.blockPropType)
        //    {
        //        catData_Temp = catNeedBlock[i];
        //        return true;
        //    }
        //}
        return false;
    }

    //检查猫咪需求
    public void CheckCatRequirements(CatData catData)
    {
        for (int i = 0; i < dropZoneData.Count; i++)
        {
            if (dropZoneData[i].GetComponent<DropZone>().blockPropTypeNew == catData.needBlock.blockPropType)
            {
                catData.UpdateTMP();
            }
        }
    }


    #endregion

    #region 道具的使用

    //清除道具
    public void ClearPropUse()
    {
        UpdateCatNeedNum();
        dropZoneData.Clear();
        for (int i = 0; i < dropZoneTran.childCount; i++)
        {
            Destroy(dropZoneTran.GetChild(i).gameObject);
        }
    }

    //清除道具 刷新 猫猫需求数
    public void UpdateCatNeedNum()
    {
        HashSet<BlockPropType> encountered = new HashSet<BlockPropType>();

        for (int i = 0; i < dropZoneData.Count; i++)
        {
            if (!encountered.Contains(dropZoneData[i].GetComponent<DropZone>().blockPropType) &&
                GetCatNeedBlockType(dropZoneData[i].GetComponent<DropZone>()))
            {
                encountered.Add(dropZoneData[i].GetComponent<DropZone>().blockPropType);
                UpdateCurrentCatNeedNum(dropZoneData[i].GetComponent<DropZone>());
            }
        }

    }

    //获取类型
    public bool GetCatNeedBlockType(DropZone dropZone)
    {
        for (int i = 0; i < needCatData_Temp.Count; i++)
        {
            if (dropZone.blockPropType == needCatData_Temp[i].blockPropType)
                return true;
        }
        return false;
    }

    //刷新
    public void UpdateCurrentCatNeedNum(DropZone dropZone)
    {
        for (int i = 0; i < catNeedBlock.Count; i++)
        {
            if (dropZone.blockPropTypeNew == catNeedBlock[i].needBlock.blockPropType)
            {
                catNeedBlock[i].text_NUM = 1;
                catNeedBlock[i].UpdateTMP();
            }
        }
    }

    //加速道具使用
    public void SpeedPropUse()
    {
        conveyorSpeed = 0.7f;
        keepTime = true;
        SpeedTimer();
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
                conveyorSpeed = 0.3f;
                timer = 0;
                Debug.LogError("加速结束 当前速度 0.3");
            }
        }
    }


    //透视道具使用
    public void PerspectivePropUse()
    {
        perspective = true;
    }

    public void PerspectiveTimer()
    {
        if (perspective)
        {
            perspectiveTimer += Time.deltaTime;
            if (perspectiveTimer >= perspectiveSurvivalTime)
            {
                perspective = false;
                perspectiveTimer = 0;
                Debug.LogError("透视结束");
            }
        }
    }


   
    #endregion

    void Update()
    {
        if (GameManager.Instance.pauseGame)
        {
            SpeedTimer();
            PerspectiveTimer();
        }

        
    }
}

public static class ListExtensions
{
    private static System.Random rng = new System.Random();

    public static void Shuffle<T>(this IList<T> list)
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

    public static Dictionary<int, List<T>> SplitIntoGroups<T>(this List<T> source, int groupCount)
    {
        Dictionary<int, List<T>> result = new Dictionary<int, List<T>>();

        int itemsPerGroup = Mathf.CeilToInt(source.Count / (float)groupCount);

        for (int i = 0; i < groupCount; i++)
        {
            int startIndex = i * itemsPerGroup;
            if (startIndex >= source.Count) break;

            int endIndex = Mathf.Min(startIndex + itemsPerGroup, source.Count);
            result.Add(i, source.GetRange(startIndex, endIndex - startIndex));
        }

        return result;
    }
}
