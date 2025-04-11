using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GameLevelManagement : MonoBehaviour
{
    public static GameLevelManagement Instance;

    [Header("方块道具种类")]
    public List<BlockPropData> blockPropAll;

    public List<BlockPropData> blockPropData_Temp;      //临时数据
    [Header("猫咪种类")]
    public List<CatData> catDataAll;

    [Header("猫咪需求道具")]
    public List<CatData> catNeedBlock;

    [Header("放置区数据")]
    public List<BlockPropData> dropZoneData;
    public Transform dropZoneTran;

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

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        for (int i = 0; i < blockPropAll.Count; i++)
        {
            blockPropData_Temp.Add(blockPropAll[i]);
        }

    }

    private void Start()
    {
        GameManager.Instance.pauseGame = true;
        GameManager.Instance.currentNumberCats = 0;
    }

    #region  放置区以及三消逻辑

    //生成放置区物品
    public void CreateDropZoneObject(BlockPropData _blockProp)
    {
        if (_blockProp.blockPropType == BlockPropType.Gift)
        {
            int randomID = Random.Range(0, blockPropAll.Count - 1);
            currentOBJ = Instantiate(blockPropAll[randomID].prefab, dropZoneTran);
        }
        else
        {
            currentOBJ = Instantiate(_blockProp.prefab, dropZoneTran);
        }

        currentOBJ.transform.localScale = new Vector3(0.7F, 0.7F, 0.7F);
        currentOBJ.SetActive(true);
        dropZoneData.Add(currentOBJ.GetComponent<BlockPropData>());

        CheckForMatches();


        if (CatNeedBlock(_blockProp))
            catData_Temp.UpdateTMP();

        Invoke("DetermineDropAreaFull",0.5f);
    }

    //检查物品类型
    public void CheckForMatches()
    {
        // 获取所有卡牌并按类型分组
        var cardGroups = dropZoneData.GroupBy(card => card.blockPropType).Where(group => group.Count() >= 3);

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
    IEnumerator DestroyObject(List<BlockPropData> matchedCards)
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
            if (dropZoneData[i].blockPropType == catData.needBlock.blockPropType)
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
        SpeedTimer();
        PerspectiveTimer();
    }
}
