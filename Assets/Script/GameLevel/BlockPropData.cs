using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BlockPropData : MonoBehaviour
{
    public BlockPropType blockPropType;
    public BlockHierarchy blockHierarchy;
    public GameObject prefab;
    public List<GameObject> unlockblock;      //0 左边 1 右边
    public Color unlockColor;


    public static Action JudgeThirdRowUnlockActon;
    public static Action JudgeScendRowUnlockActon;

    private void Awake()
    {
        prefab.GetComponent<Button>().onClick.AddListener(BlockClick);
    }

    //初始化
    public void InitBlock()
    {
        if (blockHierarchy == BlockHierarchy.BottomBlock)
        {
            transform.GetComponent<Image>().color = Color.white;
            transform.GetComponent<Button>().interactable = true;
        }
        else
        {
            transform.GetComponent<Image>().color = unlockColor;
            transform.GetComponent<Button>().interactable = false;
        }
    }

    //点击方块
    public void BlockClick()
    {
        gameObject.SetActive(false);
        GameLevelManagement.Instance.CreateDropZoneObject(this);
        JudgeBlockClick();
    }

    //判断方块是否解锁
    public void JudgeBlockClick()
    {
        switch (blockHierarchy)
        {
            case BlockHierarchy.BottomBlock:
                JudgeScendRowUnlockActon?.Invoke();
                //BlockGeneration.instance.CheckMiddleData();
                break;
            case BlockHierarchy.MiddleBlock:
                JudgeThirdRowUnlockActon?.Invoke();
                //BlockGeneration.instance.CheckTopData();
                break;
        }
    }

    //添加数据
    public void ButtonClickable()
    {
        transform.GetComponent<Button>().interactable = true;
        transform.GetComponent<Image>().color = Color.white;
    }
}

