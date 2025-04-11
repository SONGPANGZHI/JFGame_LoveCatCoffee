using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GameLevelManagement : MonoBehaviour
{
    public static GameLevelManagement Instance;

    [Header("�����������")]
    public List<BlockPropData> blockPropAll;

    public List<BlockPropData> blockPropData_Temp;      //��ʱ����
    [Header("è������")]
    public List<CatData> catDataAll;

    [Header("è���������")]
    public List<CatData> catNeedBlock;

    [Header("����������")]
    public List<BlockPropData> dropZoneData;
    public Transform dropZoneTran;

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

    #region  �������Լ������߼�

    //���ɷ�������Ʒ
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

    //�����Ʒ����
    public void CheckForMatches()
    {
        // ��ȡ���п��Ʋ������ͷ���
        var cardGroups = dropZoneData.GroupBy(card => card.blockPropType).Where(group => group.Count() >= 3);

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
            if (dropZoneData[i].blockPropType == catData.needBlock.blockPropType)
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
        SpeedTimer();
        PerspectiveTimer();
    }
}
