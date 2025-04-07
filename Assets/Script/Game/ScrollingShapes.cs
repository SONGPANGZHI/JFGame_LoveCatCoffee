using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingShapes : MonoBehaviour
{
    public Transform left_Top;
    public Transform left_Middle;
    public Transform left_Bottom;


    //[Header("ͼ������")]
    //public GameObject[] shapePrefabs;      // ͼ��Ԥ��������
    //public float horizontalSpacing = 200f;  // ͼ��ˮƽ���
    //public float verticalPosition = 0f;   // ͼ�δ�ֱλ��

    //[Header("�ƶ�����")]
    //public float moveSpeed = 1f;          // �ƶ��ٶ�
    //public float leftBoundary = -10f;     // ��߽�
    //public float rightSpawnPos = 10f;     // �Ҳ�����λ��

    //private Queue<GameObject> activeShapes = new Queue<GameObject>(); // ��ǰ�ͼ�ζ���


    public GameObject[] blockPrefab; // UI����Ԥ����
    public float blockWidth = 100f;
    public float moveSpeed = 100f;
    public float horizontalSpacing = 2f;
    public float leftBoundary = -10f;     // ��߽�
    public float rightSpawnPos = 10f;     // �Ҳ�����λ��
    private Queue<GameObject> activeShapes = new Queue<GameObject>(); // ��ǰ�ͼ�ζ���

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
        // ���ѡ��Ԥ����
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
        // �ƶ����з���
        MoveShapes();
        CheckBoundary();

    }

    void CheckBoundary()
    {
        if (activeShapes.Count == 0) return;
        // �����е�һ��Ԫ�أ�����ߵ�ͼ�Σ�
        GameObject leftmost = activeShapes.Peek();
        if (leftmost.GetComponent<RectTransform>().anchoredPosition.x < leftBoundary)
        {
            // ���ٳ����߽��ͼ��
            Destroy(leftmost);
            activeShapes.Dequeue();

            // ���Ҳ�������ͼ��
            Vector3 newPosition = new Vector3(rightSpawnPos, 0, 0);
            GenerateNewShape(newPosition);
        }
    }



}
