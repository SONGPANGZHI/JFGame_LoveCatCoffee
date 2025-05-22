using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorManagement : MonoBehaviour
{

    #region 新版玩法三消
    public float imageWidth = 1080F;                    // 单张图片的宽度
    public List<RectTransform> topConveyor_IMG;         // 三张图片的Transform数组
    public List<RectTransform> bottomConveyor_IMG;      // 三张图片的Transform数组
    public List<Transform> middle_Trans_8;
    public List<Transform> middle_Trans_10;
    public List<Transform> middle_Trans_12;
    public List<Transform> middle_Trans_14; 

    private List<Transform> middle_Trans_Temp = new List<Transform>();
    private void Start()
    {
        ConveyorInitData();
        InitConveyor();
        GetMiddlePos(PlayGameManagement.Instance.positionsNum);

        InitMiddleBlock();



    }

    //获取中间 位置数
    public void GetMiddlePos(int ID)
    {
        switch (ID)
        {
            case 8:
                middle_Trans_Temp = middle_Trans_8;
                break;
            case 10:
                middle_Trans_Temp = middle_Trans_10;
                break;
            case 12:
                middle_Trans_Temp = middle_Trans_12;
                break;
            case 14:
                middle_Trans_Temp = middle_Trans_14;
                break;
        }
    }

    //初始化 第一条 传送带
    public void InitConveyor()
    {
        for (int i = 0; i < 3; i++)
        {
            topConveyor_IMG[i].localPosition = new Vector3(i * imageWidth, 0, 0);               //向左移动
            bottomConveyor_IMG[i].localPosition = new Vector3(-imageWidth * i, 0, 0);           //向右移动

        }
    }

    public void ConveyorInitData()
    {
        for (int i = 0; i < 3; i++)
        {
            topConveyor_IMG[i].GetComponent<BlockGeneration>().GenerateAllBlock();
            bottomConveyor_IMG[i].GetComponent<BlockGeneration>().GenerateAllBlock();
        }
    }

    public void InitMiddleBlock()
    {
        for (int i = 0; i < middle_Trans_Temp.Count; i++)
        {
            int elementCount = Random.Range(PlayGameManagement.Instance.middleMin, PlayGameManagement.Instance.middleMax + 1); 
            GenerateMiddleBlock(i, elementCount);
        }
    }

    public void GenerateMiddleBlock(int transIndex,int element)
    {
        for (int i = 0; i < element; i++)
        {
            GameObject GO = Instantiate(PlayGameManagement.Instance.blockPrefab, middle_Trans_Temp[transIndex]);
            GO.transform.localPosition = new Vector2(0, i * 15);
            if (i >= PlayGameManagement.Instance.blockTypeNum)
            {
                int randomBlockType = UnityEngine.Random.Range(0, PlayGameManagement.Instance.blockTypeNum);
                GO.GetComponent<BlockPropData>().MiddleBlockInit(PlayGameManagement.Instance.blockTypes[randomBlockType], true);
            }
            else
                GO.GetComponent<BlockPropData>().MiddleBlockInit(PlayGameManagement.Instance.blockTypes[i],true);

            if (i < element - 1)
                GO.GetComponent<BlockPropData>().ButtonNotClickable();
            PlayGameManagement.Instance.middleAllNum += 1;
            PlayGameManagement.Instance.allMiddleBlockNum += 1; 
        }
    }

    void Update()
    {
        if (GameManager.Instance.pauseGame)
        {
            // 移动所有图片
            ConveyorMove();

            // 检查是否需要循环
            CheckLoopLeft();
            CheckLoopRight();
        }

    }

    //移动
    public void ConveyorMove()
    {
        foreach (Transform image in topConveyor_IMG)
        {
            image.Translate(Vector3.left * PlayGameManagement.Instance.conveyorSpeed * Time.deltaTime);
        }

        foreach (Transform item in bottomConveyor_IMG)
        {
            item.Translate(Vector3.right * PlayGameManagement.Instance.conveyorSpeed * Time.deltaTime);
        }
    }


    //检查向左移动
    public void CheckLoopLeft()
    {

        // 检查是否需要循环
        if (topConveyor_IMG[0].localPosition.x < -imageWidth)
        {
            topConveyor_IMG[0].GetComponent<BlockGeneration>().ClearAllBlock();
            RectTransform firstImage = topConveyor_IMG[0];
            firstImage.localPosition = new Vector3(topConveyor_IMG[2].localPosition.x + imageWidth, 0, 0);
            topConveyor_IMG[0] = topConveyor_IMG[1];
            topConveyor_IMG[1] = topConveyor_IMG[2];
            topConveyor_IMG[2] = firstImage;
            topConveyor_IMG[2].GetComponent<BlockGeneration>().GenerateAllBlock();
        }

    }

    //检查向右移动
    public void CheckLoopRight()
    {
        if (bottomConveyor_IMG[0].localPosition.x > imageWidth)
        {
            bottomConveyor_IMG[0].GetComponent<BlockGeneration>().ClearAllBlock();
            RectTransform firstImage = bottomConveyor_IMG[0];
            firstImage.localPosition = new Vector3(bottomConveyor_IMG[2].localPosition.x - imageWidth, 0, 0);
            bottomConveyor_IMG[0] = bottomConveyor_IMG[1];
            bottomConveyor_IMG[1] = bottomConveyor_IMG[2];
            bottomConveyor_IMG[2] = firstImage;
            bottomConveyor_IMG[2].GetComponent<BlockGeneration>().GenerateAllBlock();
        }

    }






    #endregion


    #region 旧版玩法三消

        //public float imageWidth = 1080F; // 单张图片的宽度

        //public List<RectTransform> leftConveyor_IMG; // 三张图片的Transform数组

        //public List<RectTransform> rightConveyor_IMG;

        //public List<RectTransform> leftConveyor_Sceond_IMG;
        //public List<RectTransform> rightConveyor_Sceond_IMG;

        //private bool firstGameLevel;
        //private bool otherGameLevel;
        //private int dictionaryArrayID = 2;


        //void Start()
        //{
        //    //// 初始化图片位置
        //    //InitConveyorLeft();
        //    //InitConveyorRight();

        //    ////初始化数据
        //    //ConveyorInitData();

        //    //NoviceInit();
        //    //GameLevelFirst();
        //    if (GameLevelManagement.Instance.currentLevelData.LevelID == 1)
        //        NoviceInit();
        //    else if (GameLevelManagement.Instance.currentLevelData.LevelID == 2)
        //        GameLevelFirst();
        //    else if (GameLevelManagement.Instance.currentLevelData.LevelID == 3)
        //        GameLevelSceond();
        //    else
        //        GameLevelSceond(true);

        //}

        ////新手教程
        //public void NoviceInit()
        //{
        //    leftConveyor_IMG[2].GetComponent<BlockGeneration>().CreateLeftConveyor(0);
        //    rightConveyor_IMG[2].GetComponent<BlockGeneration>().CreateRightConveyor(0);
        //}

        ////关卡一生成
        //public void GameLevelFirst()
        //{
        //    firstGameLevel = true;
        //    CreateConveyor();
        //    for (int i = 1; i < leftConveyor_IMG.Count; i++)
        //    {
        //        leftConveyor_IMG[i].GetComponent<BlockGeneration>().CreateLeftConveyor(i - 1);
        //    }

        //    for (int i = 1; i < rightConveyor_IMG.Count; i++)
        //    {
        //        rightConveyor_IMG[i].GetComponent<BlockGeneration>().CreateRightConveyor(i - 1);
        //    }
        //}

        ////生成第二关 其他
        //public void GameLevelSceond(bool _OtherLevel = false)
        //{
        //    if (_OtherLevel)
        //        otherGameLevel = true;

        //    InitConveyor();
        //    for (int i = 0; i < leftConveyor_IMG.Count; i++)
        //    {
        //        leftConveyor_IMG[i].GetComponent<BlockGeneration>().CreateLeftConveyor(i);
        //    }

        //    for (int i = 0; i < rightConveyor_IMG.Count; i++)
        //    {
        //        rightConveyor_IMG[i].GetComponent<BlockGeneration>().CreateRightConveyor(i);
        //    }
        //}

        ////生成传送带
        //public void CreateConveyor()
        //{
        //    for (int i = 0; i < 2; i++)
        //    {
        //        leftConveyor_Sceond_IMG[i].localPosition = new Vector3(i * imageWidth, 0, 0);
        //        rightConveyor_Sceond_IMG[i].localPosition = new Vector3(i * (-imageWidth), 0, 0);
        //    }
        //}


        ////初始化 第一条 传送带
        //public void InitConveyor()
        //{
        //    for (int i = 0; i < 3; i++)
        //    {
        //        leftConveyor_IMG[i].localPosition = new Vector3(i * imageWidth, 0, 0);
        //        rightConveyor_IMG[i].localPosition = new Vector3(-imageWidth * i, 0, 0);
        //    }
        //}

        //void Update()
        //{
        //    if (GameManager.Instance.pauseGame && !GameLevelManagement.Instance._isNovice)
        //    {
        //        // 移动所有图片
        //        if (firstGameLevel)
        //            FirstGameLevelConveyorMove();
        //        else
        //            ConveyorMove();

        //        // 检查是否需要循环
        //        CheckLoop();
        //        CheckLoopRight();
        //    }

        //}

        ////第一关卡 传送带移动
        //public void FirstGameLevelConveyorMove()
        //{
        //    foreach (Transform image in leftConveyor_Sceond_IMG)
        //    {
        //        image.Translate(Vector3.left * GameLevelManagement.Instance.conveyorSpeed * Time.deltaTime);
        //    }

        //    foreach (Transform item in rightConveyor_Sceond_IMG)
        //    {
        //        item.Translate(Vector3.right * GameLevelManagement.Instance.conveyorSpeed * Time.deltaTime);
        //    }
        //}


        ////移动
        //public void ConveyorMove()
        //{
        //    foreach (Transform image in leftConveyor_IMG)
        //    {
        //        image.Translate(Vector3.left * GameLevelManagement.Instance.conveyorSpeed * Time.deltaTime);
        //    }

        //    foreach (Transform item in rightConveyor_IMG)
        //    {
        //        item.Translate(Vector3.right * GameLevelManagement.Instance.conveyorSpeed * Time.deltaTime);
        //    }
        //}

        ////检查第一关 传送带 移动 左
        //public void CheckFirstConveyorLoop()
        //{
        //    if (leftConveyor_Sceond_IMG[0].localPosition.x < -imageWidth)
        //    {
        //        leftConveyor_Sceond_IMG[0].localPosition = new Vector3(leftConveyor_Sceond_IMG[1].localPosition.x + imageWidth, 0, 0);
        //    }

        //    if (leftConveyor_Sceond_IMG[1].localPosition.x < -imageWidth)
        //    {
        //        leftConveyor_Sceond_IMG[1].localPosition = new Vector3(leftConveyor_Sceond_IMG[0].localPosition.x + imageWidth, 0, 0);
        //    }
        //}

        ////检查第一关 传送带 移动 右
        //public void CheckRightConveyorLoop()
        //{
        //    if (rightConveyor_Sceond_IMG[0].localPosition.x > imageWidth)
        //    {
        //        rightConveyor_Sceond_IMG[0].localPosition = new Vector3(rightConveyor_Sceond_IMG[1].localPosition.x - imageWidth, 0, 0);
        //    }

        //    if (rightConveyor_Sceond_IMG[1].localPosition.x > imageWidth)
        //    {
        //        rightConveyor_Sceond_IMG[1].localPosition = new Vector3(rightConveyor_Sceond_IMG[0].localPosition.x - imageWidth, 0, 0);
        //    }
        //}

        ////检查向左移动
        //public void CheckLoop()
        //{

        //    // 检查是否需要循环
        //    if (firstGameLevel)
        //    {
        //        CheckFirstConveyorLoop();
        //    }
        //    else
        //    {
        //        if (leftConveyor_IMG[0].localPosition.x < -imageWidth)
        //        {
        //            if (otherGameLevel)
        //            {
        //                JudgeDicArrayID();
        //                leftConveyor_IMG[0].GetComponent<BlockGeneration>().ClearAllObject();
        //            }

        //            RectTransform firstImage = leftConveyor_IMG[0];
        //            firstImage.localPosition = new Vector3(leftConveyor_IMG[2].localPosition.x + imageWidth, 0, 0);
        //            leftConveyor_IMG[0] = leftConveyor_IMG[1];
        //            leftConveyor_IMG[1] = leftConveyor_IMG[2];
        //            leftConveyor_IMG[2] = firstImage;

        //            if (otherGameLevel)
        //            {
        //                leftConveyor_IMG[2].GetComponent<BlockGeneration>().CreateLeftConveyor(dictionaryArrayID);
        //            }

        //        }
        //    }

        //}

        ////检查向右移动
        //public void CheckLoopRight()
        //{

        //    if (firstGameLevel)
        //        CheckRightConveyorLoop();
        //    else
        //    {
        //        if (rightConveyor_IMG[0].localPosition.x > imageWidth)
        //        {
        //            if (otherGameLevel)
        //            {
        //                rightConveyor_IMG[0].GetComponent<BlockGeneration>().ClearAllObject();
        //            }

        //            // 将最左边的图片移到最右边
        //            RectTransform firstImage = rightConveyor_IMG[0];
        //            firstImage.localPosition = new Vector3(rightConveyor_IMG[2].localPosition.x - imageWidth, 0, 0);

        //            // 重新排序数组
        //            rightConveyor_IMG[0] = rightConveyor_IMG[1];
        //            rightConveyor_IMG[1] = rightConveyor_IMG[2];
        //            rightConveyor_IMG[2] = firstImage;

        //            if (otherGameLevel)
        //            {
        //                rightConveyor_IMG[2].GetComponent<BlockGeneration>().CreateRightConveyor(dictionaryArrayID);
        //            }
        //        }
        //    }



        //}

        ////判断当前字典ID
        //public int JudgeDicArrayID()
        //{
        //    dictionaryArrayID += 1;
        //    if (dictionaryArrayID >= GameLevelManagement.Instance.topBlockDic_Top.Count)
        //        dictionaryArrayID = 0;

        //    Debug.LogError("dictionaryArrayID" + dictionaryArrayID);
        //    return dictionaryArrayID;
        //}

        #endregion
    }


