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


    public int eachLayerNum;                            //ÿһ������ ����/6
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
        //if (PlayerPrefs.GetInt(GameManager.CurrentGameLevelKey) == 0)
        //{
        //    //���ֹؿ�
        //    currentLevelData = gameLevelDataList[0];
        //}
        //else
        //{
        //    currentLevelData = gameLevelDataList[PlayerPrefs.GetInt(GameManager.CurrentGameLevelKey)];
        //}
        currentLevelData = gameLevelDataList[1];
        blockPropData_Temp.Shuffle();                                       //���
        keyType = currentLevelData.TypeID / 2;                              //�ؼ��� ����
        eachLayerNum = currentLevelData.Amount / 6;                         //ÿһ�� �ؼ��� ����
        masteryNum = currentLevelData.Amount - currentLevelData.KeyCardNum;               //������ ����
        AllocatingRowsKeyCard();

        Debug.LogError("masteryNum" + masteryNum);

        //����è�������ƺ�������
        AddCatNeedType();




        GetKeyCard();
        MysteryCard();

        GetKeyCardLayer();
        GetSurplusCardLayer();
    }

    //������� �ؼ�����
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

        Debug.LogError("keyCardData" + keyCardData.Count);

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

        Debug.LogError("SurplusCardData" + SurplusCardData.Count);
    }

    //��ùؼ�����ÿ������
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

    //������ ��ÿһ��ֲ�
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
