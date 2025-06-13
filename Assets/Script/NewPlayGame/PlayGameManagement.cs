using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayGameManagement : MonoBehaviour
{
    public static PlayGameManagement Instance;

    public RectTransform uiTrans;
    public RectTransform middleTrans;
    public GameObject clock_OBJ;
    public TMP_Text timer_TMP;
    private float currentTime;        //游戏时长 秒数
    public TMP_Text levelTitle_TMP;        //猫咪需求文本
    private string catNeed_Str = "满足猫猫需求：";
    private string countDown_Str = "在规定时间内清空中间水果!";
    private string timerAndCat_Str = "在规定时间内<N>满足猫猫需求：";

    [Header("放置区数据")]
    public GameObject unlockGridSix;
    public bool specialGridUnlockSix;
    public GameObject unlockGridSeven;
    public bool specialGridUnlockSeven;
    public int dropZoneGridSum = 5;

    public List<GameObject> dropZoneData;
    public Transform dropZoneTran;
    public GameObject dropZonePrefab;

    public List<BlockDataConfig> blockDataConfig;
    public GameObject blockPrefab;
    public GameObject blockEffect;

    [Header("猫猫需求")]
    public List<CatSkin> catSkinList;
    public int requirementNum = 0;
    public int catRequirementSum;               //猫咪需求总数
    public GameObject catPrefab;                //猫咪预制体
    public Transform catPosTrans;               //猫咪位置
    public int catIndexID = 0;                  //猫猫ID;
    public List<CatData> cats = new List<CatData>();
    public List<CatRequirementFurite> allRequirements = new List<CatRequirementFurite>();


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
    public List<BlockDataConfig> blockTypes;
    public LevelType levelType;
    public int middleMin;
    public int middleMax;
    public float conveyorSpeed;
    public int positionsNum;
    public int blockTypeNum;
    public int blockArea;
    public int conveyorArea;
    public int levelTimer;
    public List<string> furnitureName;


    public int middleAllNum;
    public List<BlockPropData> currentMysteryBox;
    public List<Transform> blockAnimPos;

    private Animator _clockAnimator;
    private bool _isWarning; // 是否正在播放警告状态
    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        //获取方块列表
        GetBlockTypeList();

        //Application.targetFrameRate = 60;
        //恢复默认数据
        DropZoneGridInit();

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
        _clockAnimator = clock_OBJ.GetComponent<Animator>();
    }

    private void Start()
    {
        PlayerPrefs.DeleteKey("ADResurgenceKey");
        //新手引导
        BaseTools.Instance.UIAdaptive(uiTrans, middleTrans, catPosTrans.GetComponent<RectTransform>());
        //关卡模式
        DetermineLevelMode();
        Guidance();
    }

    #region 新手引导

    //新手引导开始游戏
    public void GuidancePlayGame()
    {
        UIManagement.Instance.OpenGuidancePlane();
        GuidancePlane.Instance.GuidanceInit(1,new Vector3(Screen.width/2,Screen.height/2,0));
    }

    //新手引导第二关
    public void GuidanceSecondLevel()
    {
        UIManagement.Instance.OpenGuidancePlane();
        GuidancePlane.Instance.GuidanceInit(8, new Vector3(Screen.width / 2, (Screen.height / 2) + 1300, 0));
    }

    //新手引导
    public void Guidance()
    {
        if (GuidancePlane.Instance.JudgeWhetherOpenGuide(1))
        {
            Invoke("GuidancePlayGame", 0.5f);
        }
        if (GameManager.Instance.currentGameLevel.LevelID == 2 && GuidancePlane.Instance.JudgeWhetherOpenGuide(8))
        {
            Invoke("GuidanceSecondLevel", 0.5f);
        }

        if(GameManager.Instance.currentGameLevel.LevelID == 3)
            UIManagement.Instance.reminderBoxPlane.OpenReminderBox(1);
    }

    #endregion

    #region 三消逻辑

    //生成放置区水果
    public void CreateDropZoneObject(BlockDataConfig _blockProp, bool middle)
    {
        GameObject currentOBJ = Instantiate(dropZonePrefab, dropZoneTran);
        currentOBJ.GetComponent<DropZone>().DropZoneInit(_blockProp, middle);

        dropZoneData.Add(currentOBJ);
        //OnFruitPlaced(currentOBJ.GetComponent<DropZone>());
        CheckForMatches();

    }

    //解锁放置区 第六个位置
    public void UnlockDropZoneSixthPos()
    {
        dropZoneGridSum += 1;
        unlockGridSix.SetActive(false);
        specialGridUnlockSix = true;
        UIManagement.Instance.reminderBoxPlane.OpenReminderBox();
    }

    //解锁放置区 第期个位置
    public void UnlockDropZoneSeventhPos()
    {
        if (specialGridUnlockSix)
        {
            //先判断第六个格子 是否解锁
            dropZoneGridSum += 1;
            unlockGridSeven.SetActive(false);
            specialGridUnlockSeven = true;
            UIManagement.Instance.reminderBoxPlane.OpenReminderBox();
        }
    }

    //初始化格子
    public void DropZoneGridInit()
    {
        dropZoneGridSum = 5;
        catIndexID = 0;
        unlockGridSix.SetActive(true);
        unlockGridSeven.SetActive(true);
        specialGridUnlockSeven = false;
        specialGridUnlockSix = false;
    }

    //生成移动动画
    public void CreateMoveAnim(Image fruitIMG, BlockDataConfig itemData, bool middle)
    {
        fruitIMG.transform.DOMove(blockAnimPos[dropZoneData.Count].position, 0.1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            Destroy(fruitIMG.transform.parent.gameObject);
        });

    }

    //检查物品类型
    public void CheckForMatches()
    {
        // 获取所有卡牌并按类型分组
        var cardGroups = dropZoneData
            .OrderBy(card => card.GetComponent<DropZone>().blockPropType)  // 先按类型排序
            .GroupBy(card => card.GetComponent<DropZone>().blockPropType)  // 然后分组
            .Where(group => group.Count() >= 3);  // 筛选出数量>=3的组

        if (cardGroups.Count() == 0)
            Invoke("DetermineDropAreaFull", 0.3f);

        // 处理匹配的卡牌组
        foreach (var group in cardGroups)
        {
            // 获取前三个匹配的卡牌
            var matchedCards = group.Take(3).ToList();
            BlockPropType matchedType = matchedCards[0].GetComponent<DropZone>().blockPropType;

            // 通知猫需求，每次匹配减少3个需求
            NotifyCatRequirements(matchedType, 3);
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
            card.GetComponent<DropZone>().PlayEffect();
        }

        MusicManagement.instance.PlayDestorySFX();
        Invoke("DetermineDropAreaFull", 0.5f);
    }

    //检查游戏状态
    public void DetermineDropAreaFull()
    {
        if (dropZoneData.Count >= dropZoneGridSum )
        {
            //游戏结束逻辑
            GameManager.Instance.pauseGame = false;
            if (PlayerPrefs.HasKey("ADResurgenceKey"))
                UIManagement.Instance.OpenGameOverPlane();
            else
            {
                //打开复活界面
                GameManager.Instance.pauseGame = false;
                UIManagement.Instance.commonPlane.gameObject.SetActive(true);
                UIManagement.Instance.commonPlane.OpenResurgenceInitPlane();
            }
        }

        CheckeGameOver();
    }

    //判断是否结束
    public void CheckeGameOver()
    {
        if (middleAllNum <= 0 && CheckDorpZoneMiddleBlock() && levelType == LevelType.Countdown)
        {
            //游戏结束
            UIManagement.Instance.OpenGameOverPlane(true);
        }
    }

    //生成特效
    IEnumerator BlockEffectPlay(List<GameObject> matchedCards)
    {
        yield return new WaitForSeconds(0.3f);
        foreach (var card in matchedCards)
        {
            GameObject GO = Instantiate(blockEffect, card.transform);
            StartCoroutine(DestroyEffect(GO));
        }

        //1秒销毁

    }

    //销毁特效
    IEnumerator DestroyEffect(GameObject effect)
    {
        yield return new WaitForSeconds(1);
        Destroy(effect);
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

    //判断关卡模式
    public void DetermineLevelMode()
    {
        switch (levelType)
        {
            case LevelType.Countdown:
                clock_OBJ.SetActive(true);
                //开始计时
                GetLevelTitle();
                StartGame();
                break;
            case LevelType.CatNeedNum:
                //生成猫猫
                clock_OBJ.SetActive(false);
                GetLevelTitle();
                InitCat();
                break;
            case LevelType.TimeAndCat:
                //两者都
                clock_OBJ.SetActive(true);
                GetLevelTitle();
                StartGame();
                InitCat();
                break;
        }
    }

    //标题文本文字改变
    public void GetLevelTitle()
    {
        if (levelType == LevelType.CatNeedNum)
            levelTitle_TMP.text = catNeed_Str + requirementNum + "/" + catRequirementSum;
        else if (levelType == LevelType.TimeAndCat)
            levelTitle_TMP.text = ListExtensions.LoadSprite(timerAndCat_Str) + requirementNum + "/" + catRequirementSum;
        else
            levelTitle_TMP.text = countDown_Str;


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
        levelType = GameManager.Instance.currentGameLevel.LevelType;
        catRequirementSum = GameManager.Instance.currentGameLevel.CatNum;
        levelTimer = GameManager.Instance.currentGameLevel.Time;
        if (GameManager.Instance.currentGameLevel.LevelID >= 17)
        {
            //奖励池随机抽取1-3家具
            int awardNum = Random.Range(1, 4);
            Debug.Log("awardNum ;" + awardNum);
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

    #region 关卡时间倒计时

    //开始游戏
    public void StartGame()
    {
        currentTime = levelTimer;
        UpdateTimerDisplay();
    }

    //开始游戏倒计时
    public void StartCountdown()
    {
        currentTime -= Time.deltaTime;
        UpdateTimerDisplay();

        if (currentTime <= 0)
        {
            GameManager.Instance.pauseGame = false;
            currentTime = 0;
            EndGame();
        }
    }

    private bool isPlaySFX;

    //关卡倒计时
    public void UpdateTimerDisplay()
    {
        // 计算分钟和秒数
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);

        // 格式化显示，确保两位数（01:05而不是1:5）
        timer_TMP.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        // 最后10秒警告逻辑
        if (currentTime <= 10f)
        {
            if (!_isWarning) // 防止重复触发
            {
                _isWarning = true;
                timer_TMP.color = Color.red;

                // 播放时钟动画
                _clockAnimator.enabled = true;
                _clockAnimator.Play("GameClock");

                // 播放音效
                MusicManagement.instance.PlayClockSFX();
            }
        }
        else if (_isWarning) // 如果之前是警告状态，现在恢复
        {
            _isWarning = false;
            timer_TMP.color = Color.white;

            // 停止动画
            _clockAnimator.enabled = false;
            clock_OBJ.transform.localScale = Vector3.one; // 重置缩放

            // 停止音效
            MusicManagement.instance.StopClockSFX();
        }

    }

    //播放闹钟
    public void PlayClockSFX()
    {
        
    }

    //添加时间
    public void AddTime(float extraTime)
    {
        currentTime += extraTime;
        UIManagement.Instance.reminderBoxPlane.OpenReminderBox(2);
    }

    //游戏结束
    public void EndGame()
    {
        Debug.Log("时间到！游戏结束");
        // 显示结算界面等
        UIManagement.Instance.OpenGameOverPlane();
    }

    #endregion

    #region 关卡猫猫

    //初始化猫猫
    public void InitCat()
    {
        cats.Clear();
        for (int i = 0; i < 2; i++)
        {
            GameObject GO = Instantiate(catPrefab,catPosTrans);
            GO.GetComponent<CatData>().CatDataInit(catIndexID);
            cats.Add(GO.GetComponent<CatData>());
            catIndexID += 1;
        }
    }

    // 生成新的猫需求
    public void GenerateNewCatRequirements()
    {
        catIndexID += 1;
        GameObject GO = Instantiate(catPrefab, catPosTrans);
        GO.GetComponent<CatData>().CatDataInit(catIndexID);
        cats.Add(GO.GetComponent<CatData>());
    }

    //猫的需求通知
    // 修改通知方法，添加数量参数
    private void NotifyCatRequirements(BlockPropType type, int amount)
    {
        var sortedCats = cats.OrderBy(cat => cat.priority).ToList();

        foreach (var cat in sortedCats)
        {
            bool foundMatch = false;
            foreach (Transform child in cat.furiteTrans)
            {
                var requirement = child.GetComponent<CatRequirementFurite>();
                if (requirement != null &&
                    requirement.currentRequirement.requiredType == type &&
                    requirement.currentRequired > 0)
                {
                    requirement.DecreaseRequirement(amount);
                    foundMatch = true;
                    break;
                }
            }
            if (foundMatch) break;
        }
    }

    //判断是否继续生成小猫
    public bool JuageCreateCat()
    {
        if (catIndexID >= catRequirementSum)
            return true;
        return false;
    }

    #endregion

    #region 道具的使用

    //清除道具
    public void ClearPropUse()
    {
        dropZoneData.Clear();
        for (int i = 0; i < dropZoneTran.childCount; i++)
        {
            dropZoneTran.GetChild(i).GetComponent<DropZone>().PlayEffect();
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
                conveyorSpeed = GameManager.Instance.currentGameLevel.conveyorSpeed;
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
        for (int i = 0; i < currentMysteryBox.Count; i++)
        {
            if (currentMysteryBox[i] == null)
                currentMysteryBox.RemoveAt(i);
        }
        //currentMysteryBox.RemoveAll(item => item == null);
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
        if (!GameManager.Instance.pauseGame) return;

        if(levelType != LevelType.CatNeedNum)
            StartCountdown();

        SpeedTimer();
        PerspectiveTimer();

       
    }
}
