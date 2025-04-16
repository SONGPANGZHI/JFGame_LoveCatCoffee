using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorManagement : MonoBehaviour
{
    public float scrollSpeed = 1.0f;
    public float imageWidth = 1080F; // 单张图片的宽度

    public List<RectTransform> leftConveyor_IMG; // 三张图片的Transform数组

    public List<RectTransform> rightConveyor_IMG;

    public List<RectTransform> leftConveyor_Sceond_IMG;
    public List<RectTransform> rightConveyor_Sceond_IMG;

    private bool firstGameLevel;
    private bool otherGameLevel;
    private int dictionaryArrayID = 2;
    void Start()
    {
        //// 初始化图片位置
        //InitConveyorLeft();
        //InitConveyorRight();

        ////初始化数据
        //ConveyorInitData();

        //NoviceInit();
        //GameLevelFirst();
        if (GameLevelManagement.Instance.currentLevelData.LevelID == 1)
            NoviceInit();
        else if (GameLevelManagement.Instance.currentLevelData.LevelID == 2)
            GameLevelFirst();
        else if(GameLevelManagement.Instance.currentLevelData.LevelID == 3)
            GameLevelSceond();
        else
            GameLevelSceond(true);

    }

    //新手教程
    public void NoviceInit()
    {
        leftConveyor_IMG[2].GetComponent<BlockGeneration>().CreateLeftConveyor(0);
        rightConveyor_IMG[2].GetComponent<BlockGeneration>().CreateRightConveyor(0);
    }

    //关卡一生成
    public void GameLevelFirst()
    {
        firstGameLevel = true;
        CreateConveyor();
        for (int i = 1; i < leftConveyor_IMG.Count; i++)
        {
            leftConveyor_IMG[i].GetComponent<BlockGeneration>().CreateLeftConveyor(i-1);
        }

        for (int i = 1; i < rightConveyor_IMG.Count; i++)
        {
            rightConveyor_IMG[i].GetComponent<BlockGeneration>().CreateRightConveyor(i-1);
        }
    }

    //生成第二关 其他
    public void GameLevelSceond(bool _OtherLevel = false)
    {
        if(_OtherLevel)
            otherGameLevel = true;

        InitConveyor();
        for (int i = 0; i < leftConveyor_IMG.Count; i++)
        {
            leftConveyor_IMG[i].GetComponent<BlockGeneration>().CreateLeftConveyor(i);
        }

        for (int i = 0; i < rightConveyor_IMG.Count; i++)
        {
            rightConveyor_IMG[i].GetComponent<BlockGeneration>().CreateRightConveyor(i);
        }
    }

    //生成传送带
    public void CreateConveyor()
    {
        for (int i = 0; i < 2; i++)
        {
            leftConveyor_Sceond_IMG[i].localPosition = new Vector3(i*imageWidth, 0, 0);
            rightConveyor_Sceond_IMG[i].localPosition = new Vector3(i*(-imageWidth), 0, 0);
        }
    }


    //初始化 第一条 传送带
    public void InitConveyor()
    {
        for (int i = 0; i < 3; i++)
        {
            leftConveyor_IMG[i].localPosition = new Vector3(i * imageWidth, 0, 0);
            rightConveyor_IMG[i].localPosition = new Vector3(-imageWidth * i, 0, 0);
        }
    }

    void Update()
    {
        if (GameManager.Instance.pauseGame && !GameLevelManagement.Instance._isNovice)
        {
            // 移动所有图片
            if (firstGameLevel)
                FirstGameLevelConveyorMove();
            else
                ConveyorMove();

            // 检查是否需要循环
            CheckLoop();
            CheckLoopRight();
        }

    }

    //第一关卡 传送带移动
    public void FirstGameLevelConveyorMove()
    {
        foreach (Transform image in leftConveyor_Sceond_IMG)
        {
            image.Translate(Vector3.left * GameLevelManagement.Instance.conveyorSpeed * 10 * Time.deltaTime);
        }

        foreach (Transform item in rightConveyor_Sceond_IMG)
        {
            item.Translate(Vector3.right * GameLevelManagement.Instance.conveyorSpeed * 10 * Time.deltaTime);
        }
    }


    //移动
    public void ConveyorMove()
    {
        foreach (Transform image in leftConveyor_IMG)
        {
            image.Translate(Vector3.left * GameLevelManagement.Instance.conveyorSpeed * 10 * Time.deltaTime);
        }

        foreach (Transform item in rightConveyor_IMG)
        {
            item.Translate(Vector3.right * GameLevelManagement.Instance.conveyorSpeed * 10 * Time.deltaTime);
        }
    }

    //检查第一关 传送带 移动 左
    public void CheckFirstConveyorLoop()
    {
        if (leftConveyor_Sceond_IMG[0].localPosition.x < -imageWidth)
        {
            leftConveyor_Sceond_IMG[0].localPosition = new Vector3(leftConveyor_Sceond_IMG[1].localPosition.x + imageWidth, 0,0);
        }

        if (leftConveyor_Sceond_IMG[1].localPosition.x < -imageWidth)
        {
            leftConveyor_Sceond_IMG[1].localPosition = new Vector3(leftConveyor_Sceond_IMG[0].localPosition.x + imageWidth,0, 0);
        }
    }

    //检查第一关 传送带 移动 右
    public void CheckRightConveyorLoop()
    {
        if (rightConveyor_Sceond_IMG[0].localPosition.x > imageWidth)
        {
            rightConveyor_Sceond_IMG[0].localPosition = new Vector3(rightConveyor_Sceond_IMG[1].localPosition.x - imageWidth, 0, 0);
        }

        if (rightConveyor_Sceond_IMG[1].localPosition.x > imageWidth)
        {
            rightConveyor_Sceond_IMG[1].localPosition = new Vector3(rightConveyor_Sceond_IMG[0].localPosition.x - imageWidth, 0, 0);
        }
    }

    //检查向左移动
    public void CheckLoop()
    {

        // 检查是否需要循环
        if (firstGameLevel)
        {
            CheckFirstConveyorLoop();
        }
        else
        {
            if (leftConveyor_IMG[0].localPosition.x < -imageWidth)
            {
                if (otherGameLevel)
                {
                    JudgeDicArrayID();
                    leftConveyor_IMG[0].GetComponent<BlockGeneration>().ClearAllObject();
                }

                RectTransform firstImage = leftConveyor_IMG[0];
                firstImage.localPosition = new Vector3(leftConveyor_IMG[2].localPosition.x + imageWidth, 0, 0);
                leftConveyor_IMG[0] = leftConveyor_IMG[1];
                leftConveyor_IMG[1] = leftConveyor_IMG[2];
                leftConveyor_IMG[2] = firstImage;

                if (otherGameLevel)
                {
                    leftConveyor_IMG[2].GetComponent<BlockGeneration>().CreateLeftConveyor(dictionaryArrayID);
                }

            }
        }

    }

    //检查向右移动
    public void CheckLoopRight()
    {

        if (firstGameLevel)
            CheckRightConveyorLoop();
        else 
        {
            if (rightConveyor_IMG[0].localPosition.x > imageWidth)
            {
                if (otherGameLevel)
                {
                    rightConveyor_IMG[0].GetComponent<BlockGeneration>().ClearAllObject();
                }

                // 将最左边的图片移到最右边
                RectTransform firstImage = rightConveyor_IMG[0];
                firstImage.localPosition = new Vector3(rightConveyor_IMG[2].localPosition.x - imageWidth, 0, 0);

                // 重新排序数组
                rightConveyor_IMG[0] = rightConveyor_IMG[1];
                rightConveyor_IMG[1] = rightConveyor_IMG[2];
                rightConveyor_IMG[2] = firstImage;

                if (otherGameLevel)
                {
                    rightConveyor_IMG[2].GetComponent<BlockGeneration>().CreateRightConveyor(dictionaryArrayID);
                }
            }
        }


       
    }

    //判断当前字典ID
    public int JudgeDicArrayID()
    {
        dictionaryArrayID += 1;
        if(dictionaryArrayID>=GameLevelManagement.Instance.topBlockDic_Top.Count)
            dictionaryArrayID = 0;

        Debug.LogError("dictionaryArrayID" + dictionaryArrayID);
        return dictionaryArrayID;
    }
}


