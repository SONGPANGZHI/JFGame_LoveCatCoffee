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
    public bool _isNovice;

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
    }

    //������� �ؼ�����
    public void AllocatingRowsKeyCard()
    {
        middleLayerKeyCardNum = (int)Math.Ceiling(currentLevelData.KeyCardNum * currentLevelData.CardType[0].InterLayer);       
        bottomLayerKeyCardNum = (int)Math.Ceiling(currentLevelData.KeyCardNum * currentLevelData.CardType[0].BottomLayer);      
        topLayerKeyCardNum = currentLevelData.KeyCardNum - middleLayerKeyCardNum - bottomLayerKeyCardNum;                       


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
