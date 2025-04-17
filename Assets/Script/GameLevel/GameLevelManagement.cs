using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class GameLevelManagement : MonoBehaviour
{
    public static GameLevelManagement Instance;
    [Header("�ؿ�����")]
    public List<GameLevelConfig> gameLevelDataList;

    [Header("�����������")]
    public List<BlockPropData> blockPropAll;

    [Header("è������")]
    public List<CatData> catDataAll;

    [Header("è���������")]
    public List<CatData> catNeedBlock;

    [Header("����������")]
    public List<GameObject> dropZoneData;
    public Transform dropZoneTran;
    public GameObject dropZonePrefab;

    private CatData catData_Temp;
    private GameObject currentOBJ;

    [Header("���ʹ� �ٶ�")]
    public int conveyorSpeed = 6;
    public bool keepTime = false;
    private float timer;

    [Header("�����ٶ� ����ʱ�� Ĭ��30s")]
    public float speedSurvivalTime = 30F ;

    [Header("͸�ӵ���")]
    public bool perspective = false;
    private float perspectiveTimer;
    [Header("͸�ӵ��� ����ʱ�� Ĭ��30s")]
    public float perspectiveSurvivalTime = 30f;


    [Header("�ؿ�����")]
    public List<BlockPropData> blockPropData_Temp;      //��ʱ����
    public GameLevelConfig currentLevelData;
    public bool _isNovice;

    public Dictionary<int, List<BlockPropData>> topBlockDic_Top = new Dictionary<int, List<BlockPropData>>();
    public Dictionary<int, List<BlockPropData>> middleBlockDic_Top = new Dictionary<int, List<BlockPropData>>();
    public Dictionary<int, List<BlockPropData>> bottomBlockDic_Top = new Dictionary<int, List<BlockPropData>>();
    public Dictionary<int, List<BlockPropData>> topBlockDic_Bottom = new Dictionary<int, List<BlockPropData>>();
    public Dictionary<int, List<BlockPropData>> middleBlockDic_Bottom = new Dictionary<int, List<BlockPropData>>();
    public Dictionary<int, List<BlockPropData>> bottomBlockDic_Bottom = new Dictionary<int, List<BlockPropData>>();

    public Dictionary<int, List<bool>> topBlockDic_Bool_Top = new Dictionary<int, List<bool>>();
    public Dictionary<int, List<bool>> middleBlockDic_Bool_Top = new Dictionary<int, List<bool>>();
    public Dictionary<int, List<bool>> bottomBlockDic_Bool_Top = new Dictionary<int, List<bool>>();

    public Dictionary<int, List<bool>> topBlockDic_Bool_Bottom = new Dictionary<int, List<bool>>();
    public Dictionary<int, List<bool>> middleBlockDic_Bool_Bottom = new Dictionary<int, List<bool>>();
    public Dictionary<int, List<bool>> bottomBlockDic_Bool_Bottom = new Dictionary<int, List<bool>>();

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

    #region ��ȡ�ؿ�����

    public List<BlockPropData> keyCardData;
    public List<BlockPropData> SurplusCardData;
    public List<BlockPropData> SurplusCardType;

    public List<BlockPropData> topLayerData_Temp;
    public List<BlockPropData> middleLayerData_Temp;
    public List<BlockPropData> bottomLayerData_Temp;

    public List<BlockPropData> needCatData_Temp;            //è��������


    public int eachLayerNum;                            //ÿһ������ ����/3
    public int keyCardAllNum;
    public int keyType;
    public int masteryNum;

    private int topLayerKeyCardNum;
    private int middleLayerKeyCardNum;
    private int bottomLayerKeyCardNum;

    private int middleLayerSurplusCardNum;
    private int bottomLayerSurplusCardNum;
    private int topLayerSurplusCardNum;

    //���� �ؿ�����
    public void LoadGameLevel()
    {
        if (PlayerPrefs.GetInt(GameManager.CurrentGameLevelKey) == 0)
        {
            //���ֹؿ�
            _isNovice = true;
        }
        else
        {
            _isNovice = false;
        }
        currentLevelData = gameLevelDataList[PlayerPrefs.GetInt(GameManager.CurrentGameLevelKey)];
        blockPropData_Temp.Shuffle();                                       //���
        keyType = currentLevelData.TypeID / 2;                              //�ؼ��� ����
        eachLayerNum = currentLevelData.Amount / 3;                         //ÿһ�� �ؼ��� ����
        masteryNum = currentLevelData.Amount - currentLevelData.KeyCardNum;               //������ ����
        AllocatingRowsKeyCard();

        //����è�������ƺ�������
        AddCatNeedType();
        //��� �ؼ��� �� ������
        GetKeyCard();
        MysteryCard();
        //���ÿһ������
        GetKeyCardLayer();
        GetSurplusCardLayer();

        //��ӵ� �ֵ�
        //InitEachLayerBlockList();

        SegmentationTopData();
        SegmentationMiddleData();
        SegmentationBottomData();
    }

    //������� �ؼ�����
    public void AllocatingRowsKeyCard()
    {
        middleLayerKeyCardNum = (int)Math.Ceiling(currentLevelData.KeyCardNum * currentLevelData.CardType[0].InterLayer);       
        topLayerKeyCardNum = (int)Math.Ceiling(currentLevelData.KeyCardNum * currentLevelData.CardType[0].TopLayer);
        bottomLayerKeyCardNum = currentLevelData.KeyCardNum - middleLayerKeyCardNum - topLayerKeyCardNum;


        middleLayerSurplusCardNum = eachLayerNum - middleLayerKeyCardNum;                     
        bottomLayerSurplusCardNum = eachLayerNum - bottomLayerKeyCardNum;                     
        topLayerSurplusCardNum = eachLayerNum - topLayerKeyCardNum;

    }


    //���è����������
    public void AddCatNeedType()
    {
        //���è�������� �ؼ���������
        for (int i = 0; i < keyType; i++)
        {
            needCatData_Temp.Add(blockPropData_Temp[i]);
        }

        //ʣ����䵽 ������
        for (int i = keyType; i < currentLevelData.TypeID; i++)
        {
            SurplusCardType.Add(blockPropData_Temp[i]);
        }
    }


    //��ȡ 
    public void GetKeyCard()
    {
        //��ӹؼ���
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

    //������
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

    //��ùؼ�����ÿ������
    public void GetKeyCardLayer()
    {

        for (int i = 0; i < topLayerKeyCardNum; i++)
        {
            topLayerData_Temp.Add(keyCardData[i]);
        }

        for (int i = 0; i < middleLayerKeyCardNum; i++)
        {
            middleLayerData_Temp.Add(keyCardData[i]);
        }

        for (int i = 0; i < bottomLayerKeyCardNum; i++)
        {
            bottomLayerData_Temp.Add(keyCardData[i]);
        }
    }

    //������ ��ÿһ��ֲ�
    public void GetSurplusCardLayer()
    {

        for (int i = 0; i < topLayerSurplusCardNum; i++)
        {
            topLayerData_Temp.Add(SurplusCardData[i]);
        }

        for (int i = 0; i < middleLayerSurplusCardNum; i++)
        {
            middleLayerData_Temp.Add(SurplusCardData[i]);
        }

        for (int i = 0; i < bottomLayerSurplusCardNum; i++)
        {
            bottomLayerData_Temp.Add(SurplusCardData[i]);
        }

        topLayerData_Temp.Shuffle();
        middleLayerData_Temp.Shuffle();
        bottomLayerData_Temp.Shuffle();
    }


    //�ָ�top����
    public void SegmentationTopData()
    {
        topLayerData_Temp.Shuffle();
        List<BlockPropData> topLayerData_Top_Temp = new List<BlockPropData>();
        List<BlockPropData> topLayerData_Bottom_Temp = new List<BlockPropData>();


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

        Debug.LogError("topBlockDic_Top" + topBlockDic_Top.Count);
        Debug.LogError("topBlockDic_Bottom" + topBlockDic_Bottom.Count);
    }

    //�ָ� middle ����
    public void SegmentationMiddleData()
    {
        middleLayerData_Temp.Shuffle();
        List<BlockPropData> topLayerData_Top_Temp = new List<BlockPropData>();
        List<BlockPropData> topLayerData_Bottom_Temp = new List<BlockPropData>();
        
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
        InitializeDictionary(arrayID,6);
        Debug.LogError("middleBlockDic_Top" + middleBlockDic_Top.Count);
        Debug.LogError("middleBlockDic_Bottom" + middleBlockDic_Bottom.Count);
    }

    //�ָ� bottom ����
    public void SegmentationBottomData()
    {
        bottomLayerData_Temp.Shuffle();
        List<BlockPropData> topLayerData_Top_Temp = new List<BlockPropData>();
        List<BlockPropData> topLayerData_Bottom_Temp = new List<BlockPropData>();
       
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
        Debug.LogError("bottomBlockDic_Top" + bottomBlockDic_Top.Count);
        Debug.LogError("bottomBlockDic_Bottom" + bottomBlockDic_Bottom.Count);
    }


    // ��ʼ��ָ�����ȵ��ֵ䣬����boolֵĬ��Ϊfalse
    public void InitializeDictionary(int keyCount, int listLength)
    {
        topBlockDic_Bool_Top.Clear();
        middleBlockDic_Bool_Top.Clear();
        bottomBlockDic_Bool_Top.Clear();
        topBlockDic_Bool_Bottom.Clear();
        middleBlockDic_Bool_Bottom.Clear();
        bottomBlockDic_Bool_Bottom.Clear();
        for (int i = 0; i < keyCount; i++)
        {
            // ������������б�
            List<bool> newList = new List<bool>(listLength);
            for (int j = 0; j < listLength; j++)
            {
                newList.Add(true); // Ĭ��ȫ��false
            }
            topBlockDic_Bool_Top.Add(i, newList);
            middleBlockDic_Bool_Top.Add(i, newList);
            bottomBlockDic_Bool_Top.Add(i, newList);
            topBlockDic_Bool_Bottom.Add(i, newList);
            middleBlockDic_Bool_Bottom.Add(i, newList);
            bottomBlockDic_Bool_Bottom.Add(i, newList);

        }
    }


    public void ModifyBlockByIndex(int dictKey, int listIndex, bool newData)
    {
        //��ȡ��Ӧ���б�
        List<bool> targetList = bottomBlockDic_Bool_Top[dictKey];

        //newData = targetList[listIndex];
        targetList[listIndex] = newData;
        Test(bottomBlockDic_Bool_Top);
        Test(topBlockDic_Bool_Top);
        Test(middleBlockDic_Bool_Top);
        //Debug.LogError("bottomBlockDic_Bool_Top" + bottomBlockDic_Bool_Top.Values);
        //Debug.LogError("topBlockDic_Bool_Top" + topBlockDic_Bool_Top.Values);
        //Debug.LogError("middleBlockDic_Bool_Top" + middleBlockDic_Bool_Top.Values);
    }

    public void Test( Dictionary<int, List<bool>> TEMP)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        foreach (var item in TEMP)
        {
            sb.AppendLine($"�� {item.Key}: " + string.Join(", ", item.Value));
            
        }
        Debug.LogError(sb);
    }

    #endregion


    #region  �������Լ������߼�

    //���ɷ�������Ʒ
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

    //�����Ʒ����
    public void CheckForMatches()
    {
        // ��ȡ���п��Ʋ������ͷ���
        var cardGroups = dropZoneData.GroupBy(card => card.GetComponent<DropZone>().blockPropType).Where(group => group.Count() >= 3);

        // ����ƥ��Ŀ�����
        foreach (var group in cardGroups)
        {
            // ��ȡǰ����ƥ��Ŀ���
            var matchedCards = group.Take(3).ToList();

            // ���ٿ��ƻ�ִ����������
            StartCoroutine(DestroyObject(matchedCards));

            // ������������ӵ÷��߼���
            Debug.Log($"������3��{group.Key}���͵Ŀ���");
        }

        // ��������ʣ�࿨��
        RearrangeCards();
        //DetermineDropAreaFull();
    }

    //��������
    private void RearrangeCards()
    {
        // ��ȡ���п��Ʋ���˳������
        var cards = dropZoneData
            .OrderBy(card => card.transform.GetSiblingIndex())
            .ToList();

        // ��������˳��
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].transform.SetSiblingIndex(i);
        }
    }



    //�����Ϸ״̬
    public void DetermineDropAreaFull()
    {
        if (dropZoneData.Count >= 7)
        {
            //��Ϸ�����߼�
            GameManager.Instance.pauseGame = false;
            UIManagement.Instance.OpenGameOverPlane();
            Debug.LogError("��Ϸ����---");
        }
    }

    //1�������
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

    #region  è������

    //è������
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

    //���è������
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

    #region ���ߵ�ʹ��

    //�������
    public void ClearPropUse()
    {
        dropZoneData.Clear();
        for (int i = 0; i < dropZoneTran.childCount; i++)
        {
            Destroy(dropZoneTran.GetChild(i).gameObject);
        }
    }

    //���ٵ���ʹ��
    public void SpeedPropUse()
    {
        conveyorSpeed = 15;
        keepTime = true;
        SpeedTimer();
        Debug.LogError("��ʼ���� ��ǰ�ٶ� 15");
    }

    //�ٶȼ�ʱ��
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
                Debug.LogError("���ٽ��� ��ǰ�ٶ� 6");
            }
        }
    }


    //͸�ӵ���ʹ��
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
                Debug.LogError("͸�ӽ���");
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
