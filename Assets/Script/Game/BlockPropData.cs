using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockPropData : MonoBehaviour
{
    public BlockPropType blockPropType;
    public BlockHierarchy blockHierarchy;
    public GameObject prefab;
    public List<GameObject> unlockblock;      //0 左边 1 右边
    public Color unlockColor;
    private void Awake()
    {
        prefab.GetComponent<Button>().onClick.AddListener(BlockClick);
    }

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
        GameManager.Instance.CreateDropZoneObject(this);
    }


    //判断方块是否解锁
    public void JudgeBlockClick()
    {
        
    }
}

