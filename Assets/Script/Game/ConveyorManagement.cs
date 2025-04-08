using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorManagement : MonoBehaviour
{
    public float scrollSpeed = 1.0f;
    public float imageWidth = 1080F; // 单张图片的宽度
    public RectTransform[] conveyor_IMG; // 三张图片的Transform数组

    private Vector3 startPosition;
    private float newPosition;

    void Start()
    {
        // 初始化图片位置
        ConveyorInitPos();
        //初始化数据
        ConveyorInitData();

    }

    //传送带 初始化位置
    public void ConveyorInitPos()
    {
        conveyor_IMG[0].anchoredPosition = new Vector3(0, -120, 0);
        conveyor_IMG[1].anchoredPosition = new Vector3(imageWidth, -120, 0);
        conveyor_IMG[2].anchoredPosition = new Vector3(imageWidth * 2, -120, 0);
    }

    //传送带 初始化 数据
    public void ConveyorInitData()
    {
        for (int i = 0; i < conveyor_IMG.Length; i++)
        {
            conveyor_IMG[i].GetComponent<BlockGeneration>().InitBlock();
        }
    }


    void Update()
    {
        // 移动所有图片
        ConveyorMove();
        // 检查是否需要循环
        CheckLoop();
    }

    public void ConveyorMove()
    {
        foreach (Transform image in conveyor_IMG)
        {
            image.Translate(Vector3.left * scrollSpeed * Time.deltaTime);
        }
    }


    public void CheckLoop()
    {
        // 检查是否需要循环
        if (conveyor_IMG[0].localPosition.x < -imageWidth)
        {
            // 将最左边的图片移到最右边
            conveyor_IMG[0].GetComponent<BlockGeneration>().ClearAllObject();
            RectTransform firstImage = conveyor_IMG[0];
            firstImage.anchoredPosition = new Vector3(conveyor_IMG[2].localPosition.x + imageWidth, -120, 0);

            // 重新排序数组
            conveyor_IMG[0] = conveyor_IMG[1];
            conveyor_IMG[1] = conveyor_IMG[2];
            conveyor_IMG[2] = firstImage;
            conveyor_IMG[2].GetComponent<BlockGeneration>().InitBlock();
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
