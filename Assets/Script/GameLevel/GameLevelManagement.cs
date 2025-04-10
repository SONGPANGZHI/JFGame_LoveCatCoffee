using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameLevelManagement : MonoBehaviour
{
    public static GameLevelManagement Instance;

    [Header("方块道具种类")]
    public List<BlockPropData> blockPropAll;
    [Header("猫咪种类")]
    public List<CatData> catDataAll;

    [Header("猫咪需求道具")]
    public List<CatData> catNeedBlock;


    [Header("放置区数据")]
    public List<BlockPropData> dropZoneData;
    public Transform dropZoneTran;

    private CatData catData_Temp;
    private GameObject currentOBJ;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    #region  放置区以及三消逻辑

    //生成放置区物品
    public void CreateDropZoneObject(BlockPropData _blockProp)
    {
        if (_blockProp.blockPropType == BlockPropType.Gift)
        {
            int randomID = Random.Range(0, blockPropAll.Count);
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

        if (dropZoneData.Count >= 7)
        {
            //游戏结束逻辑
            Debug.LogError("游戏结束---");
        }

    }

    //判断 是否 是特殊方块
    public void JudgeSpecialBlock(BlockPropData _blockProp)
    {

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

    //1秒后销毁
    IEnumerator DestroyObject(List<BlockPropData> matchedCards)
    {
        yield return new WaitForSeconds(0.3f);
        foreach (var card in matchedCards)
        {
            dropZoneData.Remove(card);
            Destroy(card.gameObject);
        }
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

}
