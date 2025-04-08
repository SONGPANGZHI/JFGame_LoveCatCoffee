using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("方块道具种类")]
    public List<BlockPropData> blockPropAll;
    [Header("猫咪种类")]
    public List<CatData> catDataAll;



    [Header("放置区数据")]
    public List<BlockPropData> dropZoneData;
    public Transform dropZoneTran;



    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }



    #region  放置区以及三消逻辑

    //生成放置区物品
    public void CreateDropZoneObject(BlockPropData _blockProp)
    {
        GameObject GO = Instantiate(_blockProp.prefab, dropZoneTran);
        GO.transform.localScale = new Vector3(0.7F, 0.7F, 0.7F);
        GO.SetActive(true);
        dropZoneData.Add(GO.GetComponent<BlockPropData>());
        if (dropZoneData.Count >= 7)
        {
            //游戏结束逻辑
            Debug.LogError("游戏结束---");
        }
        else
        {
            CheckForMatches();
        }

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

}


//[Header("猫咪设置")]
//public GameObject[] catPrefabs; // 必须包含10个不同猫咪预制体
//public Transform catsContainer;

//[Header("图形设置")]
//public GameObject[] shapePrefabs; // 必须包含10个不同图形预制体
//public Transform shapesContainer;

//[Header("放置区域")]
//public RectTransform dropZone; // 需要添加Collider2D

//private Dictionary<int, GameObject> catMap = new Dictionary<int, GameObject>(); // 图形类型到猫咪的映射
//private Dictionary<int, List<GameObject>> placedShapes = new Dictionary<int, List<GameObject>>();

//    void Start()
//    {
//        InitializeGame();
//    }

//    void InitializeGame()
//    {
//        // 确保数量匹配
//        if (catPrefabs.Length != 10 || shapePrefabs.Length != 10)
//        {
//            Debug.LogError("需要正好10个猫咪和10个图形预制体！");
//            return;
//        }

//        // 生成所有猫咪并建立映射
//        for (int i = 0; i < 10; i++)
//        {
//            GameObject cat = Instantiate(catPrefabs[i], catsContainer);
//            cat.SetActive(false);
//            catMap.Add(i, cat);
//        }

//        GenerateShapes();
//    }

//    void GenerateShapes()
//    {
//        // 生成30个图形（每个类型3个）
//        List<int> shapeIndexes = new List<int>();
//        for (int i = 0; i < 10; i++)
//        {
//            for (int j = 0; j < 3; j++)
//            {
//                shapeIndexes.Add(i);
//            }
//        }

//        // 随机打乱顺序
//        Shuffle(shapeIndexes);

//        // 实例化图形
//        foreach (int index in shapeIndexes)
//        {
//            GameObject shape = Instantiate(shapePrefabs[index], shapesContainer);
//            shape.AddComponent<DraggableShape>().Initialize(this, index);
//        }

//        // 随机显示一个猫咪
//        ShowRandomCat();
//    }

//    void ShowRandomCat()
//    {
//        // 隐藏所有猫咪
//        foreach (var cat in catMap.Values)
//        {
//            cat.SetActive(false);
//        }

//        // 随机显示一个
//        int randomIndex = Random.Range(0, 10);
//        catMap[randomIndex].SetActive(true);
//    }

//    // 被DraggableShape调用的方法
//    public void OnShapeDropped(int shapeType, GameObject shape)
//    {
//        if (!RectTransformUtility.RectangleContainsScreenPoint(dropZone, shape.transform.position))
//        {
//            return;
//        }

//        // 添加到已放置列表
//        if (!placedShapes.ContainsKey(shapeType))
//        {
//            placedShapes[shapeType] = new List<GameObject>();
//        }
//        placedShapes[shapeType].Add(shape);

//        // 检查是否满足条件
//        if (placedShapes[shapeType].Count == 3)
//        {
//            HandleMatch(shapeType);
//        }
//    }

//    void HandleMatch(int matchedType)
//    {
//        // 隐藏对应的猫咪
//        if (catMap.ContainsKey(matchedType))
//        {
//            Destroy(catMap[matchedType]);
//            catMap.Remove(matchedType);
//        }

//        // 销毁对应的图形
//        foreach (var shape in placedShapes[matchedType])
//        {
//            Destroy(shape);
//        }
//        placedShapes.Remove(matchedType);

//        // 生成新猫咪
//        ShowRandomCat();
//    }

//    void Shuffle<T>(List<T> list)
//    {
//        for (int i = 0; i < list.Count; i++)
//        {
//            T temp = list[i];
//            int randomIndex = Random.Range(i, list.Count);
//            list[i] = list[randomIndex];
//            list[randomIndex] = temp;
//        }
//    }
//}
//// 拖拽组件
//public class DraggableShape : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
//{
//    private GameManager gameManager;
//    private int shapeType;
//    private CanvasGroup canvasGroup;
//    private RectTransform rectTransform;
//    private Vector2 startPosition;

//    public void Initialize(GameManager manager, int type)
//    {
//        gameManager = manager;
//        shapeType = type;
//        rectTransform = GetComponent<RectTransform>();
//        canvasGroup = gameObject.AddComponent<CanvasGroup>();
//    }

//    public void OnBeginDrag(PointerEventData eventData)
//    {
//        startPosition = rectTransform.anchoredPosition;
//        canvasGroup.blocksRaycasts = false;
//    }

//    public void OnDrag(PointerEventData eventData)
//    {
//        rectTransform.anchoredPosition += eventData.delta / GetComponentInParent<Canvas>().scaleFactor;
//    }

//    public void OnEndDrag(PointerEventData eventData)
//    {
//        canvasGroup.blocksRaycasts = true;
//        gameManager.OnShapeDropped(shapeType, gameObject);

//        // 如果未被接收则返回原位
//        if (!IsInDropZone())
//        {
//            rectTransform.anchoredPosition = startPosition;
//        }
//    }

//    private bool IsInDropZone()
//    {
//        return RectTransformUtility.RectangleContainsScreenPoint(
//            gameManager.dropZone,
//            rectTransform.position
//        );
//    }
//}
