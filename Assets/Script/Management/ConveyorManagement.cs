using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorManagement : MonoBehaviour
{
    public float imageWidth = 1080F;                    // 单张图片的宽度
    public List<RectTransform> topConveyor_IMG;         // 三张图片的Transform数组
    public List<RectTransform> bottomConveyor_IMG;      // 三张图片的Transform数组
    public List<RectTransform> animConveyor_IMG;
    public List<Transform> middle_Trans_8;
    public List<Transform> middle_Trans_10;
    public List<Transform> middle_Trans_12;
    public List<Transform> middle_Trans_14;

    private List<Transform> middle_Trans_Temp = new List<Transform>();
    private bool stopCutsceneAnim = true;
    private void Start()
    {
        PlayGameManagement.Instance.conveyorSpeed = 1.5F;
        //初始化两条传送带
        ConveyorInitData();
        InitConveyor();

        //获得中间位置
        GetMiddlePos(PlayGameManagement.Instance.positionsNum);

        //初始化中间
        InitMiddleBlock();

        Invoke("NormalSpeed",3F);
    }

    //3秒后 速度恢复正常
    public void NormalSpeed()
    {
        PlayGameManagement.Instance.conveyorSpeed = GameManager.Instance.currentGameLevel.conveyorSpeed;
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

    //初始化、传送带位置
    public void InitConveyor()
    {
        for (int i = 0; i < 3; i++)
        {
            topConveyor_IMG[i].localPosition = new Vector3((i * imageWidth) + imageWidth, 0, 0);               //向左移动
            bottomConveyor_IMG[i].localPosition = new Vector3((-imageWidth * i) - imageWidth, 0, 0);           //向右移动

        }
    }
    //初始化传送带数据
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

    public void GenerateMiddleBlock(int transIndex, int element)
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
                GO.GetComponent<BlockPropData>().MiddleBlockInit(PlayGameManagement.Instance.blockTypes[i], true);

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

            if (stopCutsceneAnim)
            {
                OpenConveyorAnim();
                CheckLoopRightAnim();
            }
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

    //过场动画
    public void OpenConveyorAnim()
    {
        animConveyor_IMG[0].Translate(Vector3.right * PlayGameManagement.Instance.conveyorSpeed * Time.deltaTime);
        animConveyor_IMG[1].Translate(Vector3.left * PlayGameManagement.Instance.conveyorSpeed * Time.deltaTime);
    }

    //检查过场动画
    public void CheckLoopRightAnim()
    {
        if (animConveyor_IMG[0].localPosition.x > imageWidth)
        {
            animConveyor_IMG[0].gameObject.SetActive(false);
            animConveyor_IMG[1].gameObject.SetActive(false);
            stopCutsceneAnim = false;
        }

            
    }

}





   




