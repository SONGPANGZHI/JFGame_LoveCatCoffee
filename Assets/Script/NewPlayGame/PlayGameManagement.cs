using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayGameManagement : MonoBehaviour
{
    public static PlayGameManagement Instance;

    [Header("放置区数据")]
    public List<GameObject> dropZoneData;
    public Transform dropZoneTran;
    public GameObject dropZonePrefab;

    public List<BlockDataConfig> blockDataConfig_TEMP;         
    public GameObject blockPrefab;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    public void CreateDropZoneObject(BlockDataConfig _blockProp)
    {
        GameObject currentOBJ = Instantiate(dropZonePrefab, dropZoneTran);
        currentOBJ.GetComponent<DropZone>().DropZoneInit(_blockProp);

        dropZoneData.Add(currentOBJ);

        CheckForMatches();


    }

    //检查物品类型
    public void CheckForMatches()
    {
        // 获取所有卡牌并按类型分组
        var cardGroups = dropZoneData
            .OrderBy(card => card.GetComponent<DropZone>().blockPropType)  // 先按类型排序
            .GroupBy(card => card.GetComponent<DropZone>().blockPropType)  // 然后分组
            .Where(group => group.Count() >= 3);  // 筛选出数量>=3的组

       
        // 处理匹配的卡牌组
        foreach (var group in cardGroups)
        {
            // 获取前三个匹配的卡牌
            var matchedCards = group.Take(3).ToList();

            //CreateParticle(matchedCards);
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

    //0.3秒后销毁
    IEnumerator DestroyObject(List<GameObject> matchedCards)
    {

        yield return new WaitForSeconds(0.3f);
        foreach (var card in matchedCards)
        {
            dropZoneData.Remove(card);
            Destroy(card.gameObject);
        }

    }
}
