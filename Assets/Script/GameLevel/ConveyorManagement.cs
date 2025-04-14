using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorManagement : MonoBehaviour
{
    public float scrollSpeed = 1.0f;
    public float imageWidth = 1080F; // 单张图片的宽度
    public RectTransform[] leftConveyor_IMG; // 三张图片的Transform数组

    public RectTransform[] rightConveyor_IMG;
    void Start()
    {
        // 初始化图片位置
        InitConveyorLeft();
        InitConveyorRight();

        //初始化数据
        ConveyorInitData();

    }

    //初始化 第一条 传送带
    public void InitConveyorLeft()
    {
        leftConveyor_IMG[0].localPosition = new Vector3(0, 0, 0);
        leftConveyor_IMG[1].localPosition = new Vector3(imageWidth, 0, 0);
        leftConveyor_IMG[2].localPosition = new Vector3(imageWidth * 2, 0, 0);
    }

    //初始化 第二条 传送带
    public void InitConveyorRight()
    {
        rightConveyor_IMG[0].localPosition = new Vector3(0, 0, 0);
        rightConveyor_IMG[1].localPosition = new Vector3(-imageWidth, 0, 0);
        rightConveyor_IMG[2].localPosition = new Vector3(-imageWidth * 2, 0, 0);
    }


    //传送带 初始化 数据
    public void ConveyorInitData()
    {
        for (int i = 0; i < leftConveyor_IMG.Length; i++)
        {
            leftConveyor_IMG[i].GetComponent<BlockGeneration>().InitBlock();
        }

        for (int i = 0; i < rightConveyor_IMG.Length; i++)
        {
            rightConveyor_IMG[i].GetComponent<BlockGeneration>().InitBlock();
        }
    }

    void Update()
    {
        if (GameManager.Instance.pauseGame)
        {
            // 移动所有图片
            ConveyorMove();

            // 检查是否需要循环
            CheckLoop();
            CheckLoopRight();
        }

        //// 移动所有图片
        //ConveyorMove();

        //// 检查是否需要循环
        //CheckLoop();
        //CheckLoopRight();
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

    //检查向左移动
    public void CheckLoop()
    {
        // 检查是否需要循环
        if (leftConveyor_IMG[0].localPosition.x < -imageWidth)
        {
            // 将最左边的图片移到最右边
            leftConveyor_IMG[0].GetComponent<BlockGeneration>().ClearAllObject();
            RectTransform firstImage = leftConveyor_IMG[0];
            firstImage.localPosition = new Vector3(leftConveyor_IMG[2].localPosition.x + imageWidth, 0, 0);

            // 重新排序数组
            leftConveyor_IMG[0] = leftConveyor_IMG[1];
            leftConveyor_IMG[1] = leftConveyor_IMG[2];
            leftConveyor_IMG[2] = firstImage;
            leftConveyor_IMG[2].GetComponent<BlockGeneration>().InitBlock();
        }

    }

    //检查向右移动
    public void CheckLoopRight()
    {
        if (rightConveyor_IMG[0].localPosition.x > imageWidth)
        {
            // 将最左边的图片移到最右边
            rightConveyor_IMG[0].GetComponent<BlockGeneration>().ClearAllObject();
            RectTransform firstImage = rightConveyor_IMG[0];
            firstImage.localPosition = new Vector3( rightConveyor_IMG[2].localPosition.x - imageWidth, 0, 0);

            // 重新排序数组
            rightConveyor_IMG[0] = rightConveyor_IMG[1];
            rightConveyor_IMG[1] = rightConveyor_IMG[2];
            rightConveyor_IMG[2] = firstImage;
            rightConveyor_IMG[2].GetComponent<BlockGeneration>().InitBlock();
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
}
