using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameLevelManagement : MonoBehaviour
{
    public static GameLevelManagement Instance;
    [Header("关卡数据")]
    public List<GameLevelConfig> gameLevelDataList;

    [Header("方块道具种类")]
    public List<BlockPropData> blockPropAll;

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
    public int conveyorSpeed = 6;
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
    public List<BlockPropData> blockPropData_Temp;      //临时数据
    public GameLevelConfig currentLevelData;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        for (int i = 0; i < blockPropAll.Count - 1; i++)
        {
            blockPropData_Temp.Add(blockPropAll[i]);
        }

        LoadGameLevel();
    }

    private void Start()
    {
        GameManager.Instance.pauseGame = true;
        GameManager.Instance.currentNumberCats = 0;
    }

    #region 获取关卡数据

    public List<BlockPropData> keyCardData;
    public List<BlockPropData> SurplusCardData;
    public List<BlockPropData> SurplusCardType;

    public List<BlockPropData> topLayerData_Temp;
    public List<BlockPropData> middleLayerData_Temp;
    public List<BlockPropData> bottomLayerData_Temp;

    public List<BlockPropData> needCatData_Temp;            //猫咪需求牌


    public int eachLayerNum;                            //每一层数据 总数/6
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
        //if (PlayerPrefs.GetInt(GameManager.CurrentGameLevelKey) == 0)
        //{
        //    //新手关卡
        //    currentLevelData = gameLevelDataList[0];
        //}
        //else
        //{
        //    currentLevelData = gameLevelDataList[PlayerPrefs.GetInt(GameManager.CurrentGameLevelKey)];
        //}
        currentLevelData = gameLevelDataList[1];
        blockPropData_Temp.Shuffle();                                       //随机
        keyType = currentLevelData.TypeID / 2;                              //关键牌 种类
        eachLayerNum = currentLevelData.Amount / 6;                         //每一行 关键牌 总数
        masteryNum = currentLevelData.Amount - currentLevelData.KeyCardNum;               //冗余牌 总数
        AllocatingRowsKeyCard();

        Debug.LogError("masteryNum" + masteryNum);

        //分类猫咪需求牌和冗余牌
        AddCatNeedType();




        GetKeyCard();
        MysteryCard();

        GetKeyCardLayer();
        GetSurplusCardLayer();
    }

    //分配各行 关键牌数
    public void AllocatingRowsKeyCard()
    {
        middleLayerKeyCardNum = (int)Math.Ceiling(currentLevelData.KeyCardNum * currentLevelData.CardType[0].InterLayer);       //11
        bottomLayerKeyCardNum = (int)Math.Ceiling(currentLevelData.KeyCardNum * currentLevelData.CardType[0].BottomLayer);      //21
        topLayerKeyCardNum = currentLevelData.KeyCardNum - middleLayerKeyCardNum - bottomLayerKeyCardNum;                       //4


        middleLayerSurplusCardNum = masteryNum - middleLayerKeyCardNum;                     //36-11 = 25
        bottomLayerSurplusCardNum = masteryNum - bottomLayerKeyCardNum;                     //36-21 = 15
        topLayerSurplusCardNum = masteryNum - topLayerKeyCardNum;                           //36 - 4 = 32


        Debug.LogError("topLayerKeyCardNum" + topLayerKeyCardNum);
        Debug.LogError("middleLayerKeyCardNum" + middleLayerKeyCardNum);
        Debug.LogError("bottomLayerKeyCardNum" + bottomLayerKeyCardNum);

        Debug.LogError("masteryNum" + masteryNum);
        Debug.LogError("middleLayerSurplusCardNum" + middleLayerSurplusCardNum);
        Debug.LogError("bottomLayerSurplusCardNum" + bottomLayerSurplusCardNum);
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

        Debug.LogError("keyCardData" + keyCardData.Count);

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

        Debug.LogError("SurplusCardData" + SurplusCardData.Count);
    }

    //获得关键牌在每层排列
    public void GetKeyCardLayer()
    {
        for (int i = 0; i < keyCardData.Count; i++)
        {
            if (i < topLayerKeyCardNum)
            {
                topLayerData_Temp.Add(keyCardData[i]);
            }
            else if (i >= topLayerKeyCardNum&& i < middleLayerKeyCardNum)
            {
                middleLayerData_Temp.Add(keyCardData[i]);
            }
            else
            {
                bottomLayerData_Temp.Add(keyCardData[i]);
            }
        }

        Debug.LogError("topLayerData_Temp" + topLayerData_Temp.Count);
        Debug.LogError("middleLayerData_Temp" + middleLayerData_Temp.Count);
        Debug.LogError("bottomLayerData_Temp" + bottomLayerData_Temp.Count);
    }

    //其他牌 在每一层分部
    public void GetSurplusCardLayer()
    {
        for (int i = 0; i < SurplusCardData.Count; i++)
        {
            if (i < bottomLayerSurplusCardNum)
            {
                bottomLayerData_Temp.Add(SurplusCardData[i]);
            }
            else if (i >= bottomLayerSurplusCardNum && i < middleLayerSurplusCardNum)
            {
                middleLayerData_Temp.Add(SurplusCardData[i]);
            }
            else
            {
                topLayerData_Temp.Add(SurplusCardData[i]);
            }
        }

        Debug.LogError("topLayerData_Temp" + topLayerData_Temp.Count);
        Debug.LogError("middleLayerData_Temp" + middleLayerData_Temp.Count);
        Debug.LogError("bottomLayerData_Temp" + bottomLayerData_Temp.Count);
    }



    #endregion


    #region  放置区以及三消逻辑

    //生成放置区物品
    public void CreateDropZoneObject(BlockPropData _blockProp)
    {
        int randomID = UnityEngine.Random.Range(0, blockPropAll.Count - 1);
        currentOBJ = Instantiate(dropZonePrefab, dropZoneTran);
        if (_blockProp.blockPropType == BlockPropType.Gift)
        {
            currentOBJ.GetComponent<DropZone>().DropZoneInit(blockPropAll[randomID]);
            currentOBJ.name = blockPropAll[randomID].blockPropType.ToString();
        }
        else
        {
            currentOBJ.GetComponent<DropZone>().DropZoneInit(_blockProp);
            currentOBJ.name = _blockProp.blockPropType.ToString();
        }
        dropZoneData.Add(currentOBJ);
        CheckForMatches();


        if (CatNeedBlock(_blockProp))
            catData_Temp.UpdateTMP();

        Invoke("DetermineDropAreaFull",0.5f);
    }

    //检查物品类型
    public void CheckForMatches()
    {
        // 获取所有卡牌并按类型分组
        var cardGroups = dropZoneData.GroupBy(card => card.GetComponent<DropZone>().blockPropType).Where(group => group.Count() >= 3);

        // 处理匹配的卡牌组
        foreach (var group in cardGroups)
        {
            // 获取前三个匹配的卡牌
            var matchedCards = group.Take(3).ToList();

            // 销毁卡牌或执行消除动画
            StartCoroutine(DestroyObject(matchedCards));

            // 可以在这里添加得分逻辑等
            Debug.Log($"消除了3个{group.Key}类型的卡牌");
        }

        // 重新排列剩余卡牌
        RearrangeCards();
        //DetermineDropAreaFull();
    }

    //重新排列
    private void RearrangeCards()
    {
        // 获取所有卡牌并按顺序排列
        var cards = dropZoneData
            .OrderBy(card => card.transform.GetSiblingIndex())
            .ToList();

        // 重新设置顺序
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].transform.SetSiblingIndex(i);
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
            Debug.LogError("游戏结束---");
        }
    }

    //1秒后销毁
    IEnumerator DestroyObject(List<GameObject> matchedCards)
    {
        yield return new WaitForSeconds(0.3f);
        foreach (var card in matchedCards)
        {
            dropZoneData.Remove(card);
            Destroy(card.gameObject);
        }

        DetermineDropAreaFull();
    }

    #endregion

    #region  猫咪需求

    //猫咪需求
    public bool CatNeedBlock(BlockPropData _catBlock)
    {
        for (int i = 0; i < catNeedBlock.Count; i++)
        {
            if (_catBlock.blockPropType == catNeedBlock[i].needBlock.blockPropType)
            {
                catData_Temp = catNeedBlock[i];
                return true;
            }
        }
        return false;
    }

    //检查猫咪需求
    public void CheckCatRequirements(CatData catData)
    {
        for (int i = 0; i < dropZoneData.Count; i++)
        {
            if (dropZoneData[i].GetComponent<DropZone>().blockPropType == catData.needBlock.blockPropType)
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
        dropZoneData.Clear();
        for (int i = 0; i < dropZoneTran.childCount; i++)
        {
            Destroy(dropZoneTran.GetChild(i).gameObject);
        }
    }

    //加速道具使用
    public void SpeedPropUse()
    {
        conveyorSpeed = 15;
        keepTime = true;
        SpeedTimer();
        Debug.LogError("开始加速 当前速度 15");
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
                conveyorSpeed = 6;
                timer = 0;
                Debug.LogError("加速结束 当前速度 6");
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
