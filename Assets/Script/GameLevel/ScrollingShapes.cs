using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingShapes : MonoBehaviour
{
    public Transform left_Top;
    public Transform left_Middle;
    public Transform left_Bottom;


    //[Header("图形设置")]
    //public GameObject[] shapePrefabs;      // 图形预制体数组
    //public float horizontalSpacing = 200f;  // 图形水平间隔
    //public float verticalPosition = 0f;   // 图形垂直位置

    //[Header("移动设置")]
    //public float moveSpeed = 1f;          // 移动速度
    //public float leftBoundary = -10f;     // 左边界
    //public float rightSpawnPos = 10f;     // 右侧生成位置

    //private Queue<GameObject> activeShapes = new Queue<GameObject>(); // 当前活动图形队列


    public GameObject[] blockPrefab; // UI方块预制体
    public float blockWidth = 100f;
    public float moveSpeed = 100f;
    public float horizontalSpacing = 2f;
    public float leftBoundary = -10f;     // 左边界
    public float rightSpawnPos = 10f;     // 右侧生成位置
    private Queue<GameObject> activeShapes = new Queue<GameObject>(); // 当前活动图形队列

    void Start()
    {
        //blocks = new RectTransform[blockCount];

        for (int i = 0; i < 7; i++)
        {
            GameObject block = Instantiate(blockPrefab[i], left_Top);
                block.GetComponent<RectTransform>().anchoredPosition = new Vector2(i * horizontalSpacing, 0);
            activeShapes.Enqueue(block);
        }
    }

    void GenerateNewShape(Vector3 position)
    {
        // 随机选择预制体
        GameObject randomPrefab = blockPrefab[Random.Range(0, blockPrefab.Length)];
        GameObject newShape = Instantiate(randomPrefab, left_Top);
        newShape.GetComponent<RectTransform>().anchoredPosition = position;
        activeShapes.Enqueue(newShape);
    }

    public void MoveShapes()
    {
        foreach (var block in activeShapes)
        {
            block.transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }
    }

    void Update()
    {
        // 移动所有方块
        MoveShapes();
        CheckBoundary();

    }

    void CheckBoundary()
    {
        if (activeShapes.Count == 0) return;
        // 检查队列第一个元素（最左边的图形）
        GameObject leftmost = activeShapes.Peek();
        if (leftmost.GetComponent<RectTransform>().anchoredPosition.x < leftBoundary)
        {
            // 销毁超出边界的图形
            Destroy(leftmost);
            activeShapes.Dequeue();

            // 在右侧生成新图形
            Vector3 newPosition = new Vector3(rightSpawnPos, 0, 0);
            GenerateNewShape(newPosition);
        }
    }



}
